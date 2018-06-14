using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IFriendsRepository : IDisposable
    {
        IEnumerable<Friend> GetAll();
        Friend Get(int id);
        void Insert(Friend entity);
        void Delete(Friend entity);
        void Update(Friend entity);
        void Save();
    }
}
