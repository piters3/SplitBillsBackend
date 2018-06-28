using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class SubcategoriesRepository : ISubcategoriesRepository
    {
        private SplitBillsDbContext _ctx;

        public SubcategoriesRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<Subcategory> GetAll()
        {
            return _ctx.Subcategories.ToList();
        }

        public Subcategory Get(int id)
        {
            return _ctx.Subcategories.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(Subcategory entity)
        {
            _ctx.Subcategories.Add(entity);
        }

        public void Update(Subcategory entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Subcategory entity)
        {
            _ctx.Subcategories.Remove(entity);
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
