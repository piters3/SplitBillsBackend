using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<Friend> GetUserFriends(int id);
        IEnumerable<Bill> GetUserExpenses(int id);
        void Save();
    }
}
