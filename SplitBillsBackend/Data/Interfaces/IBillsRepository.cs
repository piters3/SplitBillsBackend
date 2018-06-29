using System.Collections.Generic;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Interfaces
{
    public interface IBillsRepository : IRepository<Bill>
    {
        IEnumerable<Bill> GetUserExpenses(int id);
        IEnumerable<Bill> GetCommonExpenses(int userId, int friendId);
        IEnumerable<Bill> GetBillsCreatedByUser(int id);
        IEnumerable<Bill> GetBillsInWhichUserIsPayer(int id);
    }
}
