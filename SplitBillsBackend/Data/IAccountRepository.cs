using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Data
{
    public interface IAccountRepository : IDisposable
    {
        IEnumerable<Friend> GetUserFriends(int id);
        IEnumerable<Bill> GetUserExpenses(int id);
        IEnumerable<Bill> GetCommonExpenses(int userId, int friendId);
        IEnumerable<Bill> GetBillsCreatedByUser(int id);
        IEnumerable<Bill> GetBillsInWhichUserIsPayer(int id);
        void Save();
    }
}
