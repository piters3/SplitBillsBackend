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
    }
}
