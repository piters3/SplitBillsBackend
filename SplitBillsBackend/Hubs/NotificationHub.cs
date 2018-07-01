using System;
using System.Collections.Generic;
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

        public NotificationHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SendNotification(object message)
        {
            await Clients.All.SendAsync("SendNotification", message);
        }

        public async Task SendToUser(string id, object message)
        {
            //await Clients.User(id).SendAsync("SendToUser", message);
            await Clients.Client(id).SendAsync("SendToUser", message);
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
            if (Identity != null)
            {
                var userId = Convert.ToInt32(Identity.Claims.Single(c => c.Type == Constants.JwtClaimIdentifiers.Id).Value);
                var loggingInUser = _unitOfWork.UsersRepository.Get(userId);

                loggingInUser.Connected = true;
                loggingInUser.ConnectionId = Context.ConnectionId;

                _unitOfWork.UsersRepository.Update(loggingInUser);
                _unitOfWork.Complete();
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var connecionId = Context.ConnectionId;
            var loggingOutUser = _unitOfWork.UsersRepository.Find(u => u.ConnectionId == connecionId).SingleOrDefault();

            if (loggingOutUser != null)
            {
                loggingOutUser.Connected = false;
                loggingOutUser.ConnectionId = null;

                _unitOfWork.UsersRepository.Update(loggingOutUser);
                _unitOfWork.Complete();
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}

