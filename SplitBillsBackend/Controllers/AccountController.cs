﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SplitBillsBackend.Auth;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Helpers;
using SplitBillsBackend.Hubs;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AccountController(UserManager<User> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
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
            NotificationHub.Identity = identity;

            if (identity == null)
            {
                return BadRequest();
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
            var all = _unitOfWork.FriendsRepository.GetUserFriends(id);
            var model = Mapper.Map<IEnumerable<FriendModel>>(all);
            return model;
        }


        // GET /api/Account/Expenses
        [HttpGet("Expenses")]
        public IActionResult Expenses()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _unitOfWork.BillsRepository.GetUserExpenses(id);
            var model = Mapper.Map<IEnumerable<BillModel>>(all);

            var expensesSummary = model.SelectMany(bill => bill.Payers).Sum(payer => payer.Amount);

            return new OkObjectResult(new
            {
                Expenses = model,
                ExpensesSummary = expensesSummary
            });
        }

        // GET /api/Account/CommonExpenses
        [HttpGet("CommonExpenses/{friendId:int}")]
        public IActionResult CommonExpenses(int friendId)
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _unitOfWork.BillsRepository.GetCommonExpenses(id, friendId);
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
            var billsCreatedByUser = _unitOfWork.BillsRepository.GetBillsCreatedByUser(id);
            var billsInWhichUserIsPayer = _unitOfWork.BillsRepository.GetBillsInWhichUserIsPayer(id);
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

            var bill = new Bill
            {
                Creator = _unitOfWork.UsersRepository.Get(model.CreatorId),
                Date = model.Date,
                Description = model.Description,
                Notes = model.Notes.Select(n => new Note { Id = n.Id, Text = n.Text }).ToList(),
                Subcategory = _unitOfWork.SubcategoriesRepository.Get(model.SubcategoryId),
                TotalAmount = model.TotalAmount,
                UserBills = model.Payers.Select(p => new UserBill { User = _unitOfWork.UsersRepository.Get(p.Id), Amount = p.Amount }).ToList()
            };

            _unitOfWork.BillsRepository.Add(bill);

            var history = new History
            {
                Bill = bill,
                Creator = bill.Creator,
                Date = bill.Date,
                Description = bill.Description,
                HistoryType = ActionType.Add
            };

            var notifications = new List<Notification>();

            foreach (var reader in bill.UserBills)
            {
                if (reader.User.Id != bill.Creator.Id)
                {
                    notifications.Add(new Notification
                    {
                        Readed = false,
                        Reader = reader.User,
                        History = history
                    });
                }
            }

            _unitOfWork.HistoriesRepository.Add(history);
            _unitOfWork.NotificationsRepository.AddRange(notifications);

            SendNotifications(bill, notifications);

            _unitOfWork.Complete();

            return new OkObjectResult(new
            {
                Message = "Rachunek został dodany"
            });
        }

        private void SendNotifications(Bill bill, List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                if (notification.Reader.Connected)
                {
                    var notificationModel = new NotificationModel
                    {
                        Date = bill.Date,
                        Description = bill.Description,
                        HistoryType = ActionType.Add,
                        UserName = bill.Creator.UserName,
                        Amount = bill.UserBills.Single(x => x.User.Id == notification.Reader.Id).Amount
                    };
                    _hubContext.Clients.Client(notification.Reader.ConnectionId).SendAsync("SendToUser", notificationModel);
                }
            }
            //_hubContext.Clients.All.SendAsync("SendNotification", "Asd");
        }


        // GET /api/Account/Activity
        [HttpGet("Activity")]
        public IEnumerable<HistoryModel> Activity()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _unitOfWork.HistoriesRepository.GetHistoriesForUser(id);

            var model = all.Select(history => new HistoryModel
            {
                Id = history.Id,
                Date = history.Date,
                Description = history.Description,
                HistoryType = history.HistoryType,
                UserName = history.Creator.UserName,
                Amount = history.Creator.Id == id ? history.Bill.TotalAmount : history.Bill.UserBills.Single(x => x.User.Id == id).Amount * -1
            }).ToList();

            return model;
        }


        // GET /api/Account/Notifications
        [HttpGet("Notifications")]
        public IEnumerable<NotificationModel> GetUnreadedNotifications()
        {
            var id = Convert.ToInt32(User.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            var all = _unitOfWork.NotificationsRepository.GetUnreadedNotificationsForUser(id);

            var model = all.Select(n => new NotificationModel
            {
                Date = n.History.Date,
                Description = n.History.Description,
                HistoryType = n.History.HistoryType,
                UserName = n.History.Creator.UserName,
                Amount = n.History.Creator.Id == id ? n.History.Bill.TotalAmount : n.History.Bill.UserBills.Single(x => x.User.Id == id).Amount * -1
            }).ToList();

            return model;
        }


        // POST /api/Account/ReadNotification
        [HttpPost("ReadNotification/{id}")]
        public IActionResult ReadNotification(int id)
        {
            var notification = _unitOfWork.NotificationsRepository.Get(id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.Readed = true;

            _unitOfWork.NotificationsRepository.Update(notification);
            _unitOfWork.Complete();

            return new OkObjectResult(new
            {
                Message = "Odczytano!"
            });
        }
    }
}
