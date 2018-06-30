using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Helpers;

namespace SplitBillsBackend.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        public static ClaimsIdentity Identity { get; set; }
        public static User CurrentUser { get; set; }

        public NotificationHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendNotification(object message)
        {
            await Clients.All.SendAsync("SendNotification", message);
        }

        //public async Task SendNotification(string userId, object message)
        //{
        //    await Clients.User(userId).SendAsync("SendNotification", message);
        //}

        //public async Task SendNotification(List<string> userIds, object message)
        //{
        //    await Clients.Users(userIds).SendAsync("SendNotification", message);
        //}

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = Convert.ToInt32(Identity.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
            CurrentUser = _unitOfWork.UsersRepository.Get(userId);

            CurrentUser.Connected = true;
            CurrentUser.ConnectionId = connectionId;

            _unitOfWork.UsersRepository.Update(CurrentUser);
            _unitOfWork.Complete();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            CurrentUser.Connected = false;
            CurrentUser.ConnectionId = null;

            _unitOfWork.UsersRepository.Update(CurrentUser);
            _unitOfWork.Complete();

            return base.OnDisconnectedAsync(exception);
        }
    }
}

