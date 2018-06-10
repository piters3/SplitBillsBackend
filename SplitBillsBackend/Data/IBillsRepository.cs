using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IBillsRepository : IDisposable
    {
        IEnumerable<Bill> GetAll();
        Bill Get(int id);
        void Insert(Bill entity);
        void Delete(Bill entity);
        void Update(Bill entity);
        void Save();
    }
}
