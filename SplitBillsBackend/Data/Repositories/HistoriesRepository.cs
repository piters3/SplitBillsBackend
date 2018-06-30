using System.Collections.Generic;
using System.Linq;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class HistoriesRepository : Repository<History>, IHistoriesRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public HistoriesRepository(SplitBillsDbContext context) : base(context)
        {
        }

        public IEnumerable<History> GetHistoriesForUser(int id)
        {
            return _ctx.Histories
                .Where(h => h.Creator.Id == id || h.Creator.Friends.Any(f => f.SecondFriend.Id == id))
                .OrderByDescending(h => h.Date)
                .ToList();  //distinct?
        }
    }
}
