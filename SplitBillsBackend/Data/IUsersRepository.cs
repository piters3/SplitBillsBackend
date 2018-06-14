using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IUsersRepository : IDisposable
    {
        IEnumerable<User> GetAll();
        User Get(string id);
        void Insert(User entity);
        void Delete(User entity);
        void Update(User entity);
        void Save();
    }
}
