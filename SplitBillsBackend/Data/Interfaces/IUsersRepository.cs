using System.Collections.Generic;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        List<string> GetUsersConnectionIds(List<int> userIds);
    }
}
