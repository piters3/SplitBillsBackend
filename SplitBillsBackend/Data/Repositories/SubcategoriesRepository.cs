using SplitBillsBackend.Data.Interfaces;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data.Repositories
{
    public class SubcategoriesRepository : Repository<Subcategory>, ISubcategoriesRepository
    {
        private SplitBillsDbContext _ctx => Ctx as SplitBillsDbContext;

        public SubcategoriesRepository(SplitBillsDbContext context) : base(context)
        {
        }    
    }
}
