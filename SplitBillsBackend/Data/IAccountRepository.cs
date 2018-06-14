using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<Friend> GetUserFriends(string id);
        void Save();
    }
}
