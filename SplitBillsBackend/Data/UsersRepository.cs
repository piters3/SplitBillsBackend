using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class UsersRepository : IUsersRepository
    {
        private SplitBillsDbContext _ctx;

        public UsersRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _ctx.Users
                .Include(u => u.UserBills)
                    .ThenInclude(b => b.Bill)
                    .ThenInclude(b => b.Subcategory)
                    .ThenInclude(s => s.Category)
                .Include(u => u.Friends)
                .Include(u => u.UserRoles)
                    .ThenInclude(r => r.Role)
                .ToList();
        }

        public User Get(string id)
        {
            return _ctx.Users
                 .Include(u => u.UserBills)
                     .ThenInclude(b => b.Bill)
                     .ThenInclude(b => b.Subcategory)
                     .ThenInclude(s => s.Category)
                 .Include(u => u.Friends)
                 .Include(u => u.UserRoles)
                    .ThenInclude(r => r.Role).FirstOrDefault(x => x.Id == id);
        }

        public void Insert(User entity)
        {
            _ctx.Users.Add(entity);
        }

        public void Update(User entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(User entity)
        {
            _ctx.Users.Remove(entity);
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
