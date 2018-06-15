using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class FriendsRepository : IFriendsRepository
    {
        private SplitBillsDbContext _ctx;

        public FriendsRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<Friend> GetAll()
        {
            return _ctx.Friends
                .ToList();
        }

        public Friend Get(int id)
        {
            return _ctx.Friends
                .FirstOrDefault(x => x.Id == id);
        }

        public void Insert(Friend entity)
        {
            _ctx.Friends.Add(entity);
        }

        public void Update(Friend entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Friend entity)
        {
            _ctx.Friends.Remove(entity);
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
