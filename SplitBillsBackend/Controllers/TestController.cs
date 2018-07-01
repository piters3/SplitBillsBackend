using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Hubs;
using SplitBillsBackend.Models;

namespace SplitBillsBackend.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public TestController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public IActionResult Post()
        {
            var notificationModel = new NotificationModel
            {
                Date = DateTime.Now,
                Description = "Test",
                HistoryType = ActionType.Add,
                UserName = "Testowy",
                Amount = 100
            };
            _hubContext.Clients.All.SendAsync("SendNotification", notificationModel);

            return Ok();
        }     
    }
}
