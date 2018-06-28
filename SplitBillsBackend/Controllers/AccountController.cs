using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SplitBillsBackend.Auth;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Helpers;
using SplitBillsBackend.Mappings;
using SplitBillsBackend.Models;

namespace SplitBillsBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IAccountRepository _repo;
        private readonly IBillsRepository _billsRepo;
        private readonly IUsersRepository _usersRepo;
        private readonly ISubcategoriesRepository _subcatRepo;

        public AccountController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IAccountRepository repo, IBillsRepository billsRepo, IUsersRepository usersRepo, ISubcategoriesRepository subcatRepo)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _repo = repo;
            _billsRepo = billsRepo;
            _usersRepo = usersRepo;
            _subcatRepo = subcatRepo;
        }


        // POST api/accounts
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = Mapper.Map<User>(model);

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return new OkObjectResult(new { Message = "Konto zostało utworzone" });
        }


        // POST /api/Account/login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Post([FromBody]CredentialsModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(identity);
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(userToVerify);


            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id.ToString(), roles[0]));
            }

            return await Task.FromResult<ClaimsIdentity>(null);
        }


        // GET /api/Account/Friends
        [HttpGet("Friends")]
        public IEnumerable<FriendModel> Friends()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _repo.GetUserFriends(id);
            var model = Mapper.Map<IEnumerable<FriendModel>>(all);
            return model;
        }


        // GET /api/Account/Expenses
        [HttpGet("Expenses")]
        public IActionResult Expenses()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _repo.GetUserExpenses(id);
            var model = Mapper.Map<IEnumerable<BillModel>>(all);

            var expensesSummary = model.SelectMany(bill => bill.Payers).Sum(payer => payer.Amount);

            return new OkObjectResult(new
            {
                model,
                ExpensesSummary = expensesSummary
            });
        }

        // GET /api/Account/CommonExpenses
        [HttpGet("CommonExpenses/{friendId:int}")]
        public IActionResult CommonExpenses(int friendId)
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _repo.GetCommonExpenses(id, friendId);
            var commonExpenses = Mapper.Map<IEnumerable<BillModel>>(all);

            var expensesSummary = 0.00m;

            foreach (var bill in commonExpenses)
            {
                foreach (var payer in bill.Payers)
                {
                    if (bill.Creator.Id == id && payer.Id == friendId)
                    {
                        expensesSummary += payer.Amount;
                    }
                    else if (bill.Creator.Id == friendId && payer.Id == id)
                    {
                        expensesSummary -= payer.Amount;
                    }
                }
            }

            return new OkObjectResult(new
            {
                CommonExpenses = commonExpenses,
                ExpensesSummary = expensesSummary
            });
        }

        // GET /api/Account/Dashboard
        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var billsCreatedByUser = _repo.GetBillsCreatedByUser(id);
            var billsInWhichUserIsPayer = _repo.GetBillsInWhichUserIsPayer(id);
            var userBorrowers = new List<BorrowerModel>();
            var userOwedTo = new List<BorrowerModel>();

            foreach (var bill in billsInWhichUserIsPayer)
            {
                foreach (var item in bill.UserBills)
                {
                    if (item.User.Id == id && item.Settled == false)
                    {
                        var existed = userOwedTo.FirstOrDefault(x => x.Id == bill.Creator.Id);
                        if (existed != null)
                        {
                            existed.Amount += item.Amount;
                        }
                        else
                        {
                            userOwedTo.Add(new BorrowerModel
                            {
                                Id = bill.Creator.Id,
                                Name = bill.Creator.Name,
                                SurName = bill.Creator.Surname,
                                Amount = item.Amount
                            });
                        }
                    }
                }
            }

            foreach (var bill in billsCreatedByUser)
            {
                foreach (var item in bill.UserBills)
                {
                    if (item.User.Id != id && item.Settled == false)
                    {
                        var existed = userBorrowers.FirstOrDefault(x => x.Id == item.User.Id);
                        if (existed != null)
                        {
                            existed.Amount += item.Amount;
                        }
                        else
                        {
                            userBorrowers.Add(new BorrowerModel
                            {
                                Id = item.User.Id,
                                Name = item.User.Name,
                                SurName = item.User.Surname,
                                Amount = item.Amount
                            });
                        }
                    }
                }
            }

            var owedToUserSummary = userBorrowers.Select(x => x.Amount).Sum();
            var userOwedSummary = userOwedTo.Select(x => x.Amount).Sum();

            return new OkObjectResult(new
            {
                UserBorrowers = userBorrowers,
                OwedToUserSummary = owedToUserSummary,
                UserOwedTo = userOwedTo,
                UserOwedSummary = userOwedSummary,
                TotalBalance = owedToUserSummary - userOwedSummary
            });
        }


        // POST /api/Account/AddBill
        [HttpPost("AddBill")]
        public IActionResult AddBill([FromBody]AddBillModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userBills = new List<UserBill>();

            foreach (var payer in model.Payers)
            {
                userBills.Add(new UserBill { User = _usersRepo.Get(payer.Id), Amount = payer.Amount });
            }

            var bill = new Bill
            {
                Creator = _usersRepo.Get(model.CreatorId),
                Date = model.Date,
                Description = model.Description,
                Notes = model.Notes,
                Subcategory = _subcatRepo.Get(model.SubcategoryId),
                TotalAmount = model.TotalAmount,
                UserBills = userBills
            };

            _billsRepo.Insert(bill);
            _billsRepo.Save();

            return new OkObjectResult(new
            {
                Message = "Rachunek został dodany"
            });
        }
    }
}
