using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SplitBillsBackend.Hubs;

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
            _hubContext.Clients.All.SendAsync("SendNotification", "SIEMA");

            return Ok();
        }     
    }
}
