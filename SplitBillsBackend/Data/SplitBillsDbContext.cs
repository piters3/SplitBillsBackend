using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;

namespace SplitBillsBackend.Data
{
    public class SplitBillsDbContext : IdentityDbContext<User>
    {
        public SplitBillsDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
    }
}
