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
                .Include(f=>f.SecondFriend)
                .Where(f => f.FirstFriend.Id == id)
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
