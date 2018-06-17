using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class AccountRepository : IAccountRepository
    {
        private SplitBillsDbContext _ctx;

        public AccountRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<Friend> GetUserFriends(int id)
        {
            return _ctx.Friends
                .Include(f => f.FirstFriend)
                .Include(f => f.SecondFriend)
                .Where(f => f.FirstFriend.Id == id)
                .ToList();
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

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_ctx != null)
                {
                    _ctx.Dispose();
                    _ctx = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
