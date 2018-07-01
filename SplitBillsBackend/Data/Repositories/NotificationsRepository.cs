using System.Collections.Generic;
using System.Linq;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class NotificationsRepository : Repository<Notification>, INotificationsRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public NotificationsRepository(SplitBillsDbContext context) : base(context)
        {
        }

        public IEnumerable<Notification> GetUnreadedNotificationsForUser(int userId)
        {
            return _ctx.Notifications
                .Where(n => n.Reader.Id == userId && n.Readed == false)
                .ToList();
        }
    }
}
