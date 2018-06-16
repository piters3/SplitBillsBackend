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
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IAccountRepository _repo;

        public AccountController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IAccountRepository repo)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _repo = repo;
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

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id.ToString()));
            }

            return await Task.FromResult<ClaimsIdentity>(null);
        }


        // GET /api/Account/Friends
        [HttpGet("Friends")]
        [Authorize]
        public IEnumerable<FriendModel> Friends()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == "id").Value);
            var all = _repo.GetUserFriends(id);
            var model = Mapper.Map<IEnumerable<FriendModel>>(all);
            return model;
        }


        // GET /api/Account/Expenses
        [HttpGet("Expenses")]
        [Authorize]
        public IActionResult Expenses()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == "id").Value);
            var all = _repo.GetUserExpenses(id);
            var model = Mapper.Map<IEnumerable<BillModel>>(all);

            var expensesSumary = model.SelectMany(bill => bill.Payers).Sum(payer => payer.Amount);

            return new OkObjectResult(new
            {
                model,
                ExpensesSumary = expensesSumary
            });
        }

    }
}
