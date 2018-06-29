using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class CategoriesRepository : Repository<Category>, ICategoriesRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public CategoriesRepository(SplitBillsDbContext context) : base(context)
        {
        }
    }
}
