using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class AccountRepository : IAccountRepository, IDisposable
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

        public IEnumerable<Friend> GetUserFriends(string id)
        {
            return _ctx.Friends
                .Include(f => f.FirstFriend)
                .Include(f => f.SecondFriend)
                .Where(f => f.FirstFriend.Id == id)
                .ToList();
        }

        public IEnumerable<Bill> GetUserExpenses(string id)
        {
            return _ctx.Bills
                .Include(b => b.Subcategory).ThenInclude(s => s.Category)
                .Include(b => b.UserBills).ThenInclude(user => user.User)
                .Where(b => b.UserBills.Any(c => c.User.Id == id))
                .ToList();
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
