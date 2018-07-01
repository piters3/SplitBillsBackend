using System.Collections.Generic;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Interfaces
{
    public interface INotificationsRepository : IRepository<Notification>
    {
        IEnumerable<Notification> GetUnreadedNotificationsForUser(int userId);
    }
}
