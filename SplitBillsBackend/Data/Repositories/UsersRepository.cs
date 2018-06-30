using System.Collections.Generic;
using System.Linq;
using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public UsersRepository(SplitBillsDbContext context) : base(context)
        {
        }

        public List<string> GetUsersConnectionIds(List<int> userIds)
        {
            var list = (from user in _ctx.Users
                        from id in userIds
                        where user.Id == id && user.Connected
                        select user.ConnectionId)
                        .ToList();

            return list;
        }
    }
}
