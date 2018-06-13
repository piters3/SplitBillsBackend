using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class BillsRepository : IBillsRepository, IDisposable
    {
        private SplitBillsDbContext _ctx;

        public BillsRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<Bill> GetAll()
        {
            return _ctx.Bills.Include(bill => bill.UserBills).ThenInclude(user => user.User).Include(x=>x.Subcategory).ToList();
        }

        public Bill Get(int id)
        {
            return _ctx.Bills.Include(bill => bill.UserBills).ThenInclude(user => user.User).Include(x => x.Subcategory).Where(x => x.Id == id).FirstOrDefault();
        }

        public void Insert(Bill entity)
        {
            _ctx.Bills.Add(entity);
        }

        public void Update(Bill entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Bill entity)
        {
            _ctx.Bills.Remove(entity);
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
