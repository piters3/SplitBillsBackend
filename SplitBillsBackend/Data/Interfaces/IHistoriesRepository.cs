using System.Collections.Generic;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Interfaces
{
    public interface IHistoriesRepository : IRepository<History>
    {
        IEnumerable<History> GetHistoriesForUser(int id);
    }
}
