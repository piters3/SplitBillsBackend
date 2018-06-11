using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface ICategoriesRepository : IDisposable
    {
        IEnumerable<Category> GetAll();
        Category Get(int id);
        void Insert(Category entity);
        void Delete(Category entity);
        void Update(Category entity);
        void Save();
    }
}
