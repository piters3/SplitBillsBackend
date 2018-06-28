using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface ISubcategoriesRepository : IDisposable
    {
        IEnumerable<Subcategory> GetAll();
        Subcategory Get(int id);
        void Insert(Subcategory entity);
        void Delete(Subcategory entity);
        void Update(Subcategory entity);
        void Save();
    }
}
