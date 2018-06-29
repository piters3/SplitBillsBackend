using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class BillsRepository : Repository<Bill>, IBillsRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public BillsRepository(SplitBillsDbContext context) : base(context)
        {
        }

        public IEnumerable<Bill> GetUserExpenses(int id)
        {
            return _ctx.Bills
                .Include(b => b.Subcategory).ThenInclude(s => s.Category)
                .Include(b => b.UserBills).ThenInclude(user => user.User)
                .Where(b => b.UserBills.Any(c => c.User.Id == id))
                .ToList();
        }

        public IEnumerable<Bill> GetCommonExpenses(int userId, int friendId)
        {
            var creators = _ctx.Bills
                .Include(b => b.Subcategory).ThenInclude(s => s.Category)
                .Include(b => b.UserBills).ThenInclude(user => user.User)
                .Where(b => b.Creator.Id == userId || b.Creator.Id == friendId)
                .ToList();

            var my = creators.Where(b => b.UserBills.Any(c => c.User.Id == userId));
            var friend = creators.Where(b => b.UserBills.Any(c => c.User.Id == friendId));
            return my.Intersect(friend).ToList();
        }

        public IEnumerable<Bill> GetBillsCreatedByUser(int id)
        {
            var billsCreatedByUser = _ctx.Bills
                .Include(b => b.Subcategory).ThenInclude(s => s.Category)
                .Include(b => b.UserBills).ThenInclude(user => user.User)
                .Where(b => b.Creator.Id == id)
                .ToList();

            return billsCreatedByUser;
        }

        public IEnumerable<Bill> GetBillsInWhichUserIsPayer(int id)
        {
            var billsInWhichUserIsPayer = _ctx.Bills
                .Include(b => b.Subcategory).ThenInclude(s => s.Category)
                .Include(b => b.UserBills).ThenInclude(user => user.User)
                .Where(b => b.Creator.Id != id && b.UserBills.Any(x => x.User.Id == id))
                .ToList();

            return billsInWhichUserIsPayer;
        }
    }
}
