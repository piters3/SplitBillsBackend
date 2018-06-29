using System.Collections.Generic;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Interfaces
{
    public interface IFriendsRepository : IRepository<Friend>
    {
        IEnumerable<Friend> GetUserFriends(int id);
    }
}
