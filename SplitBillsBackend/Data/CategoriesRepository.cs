using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Data;
using SplitBillsBackend.Entities;

namespace SplitCategorysBackend.Data
{
    public class CategoriesRepository : ICategoriesRepository, IDisposable
    {
        private SplitBillsDbContext _ctx;

        public CategoriesRepository(SplitBillsDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            return _ctx.Categories.Include(c => c.Subcategories).ToList();
        }

        public Category Get(int id)
        {
            return _ctx.Categories.Find(id);
        }

        public void Insert(Category entity)
        {
            _ctx.Categories.Add(entity);
        }

        public void Update(Category entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Category entity)
        {
            _ctx.Categories.Remove(entity);
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
