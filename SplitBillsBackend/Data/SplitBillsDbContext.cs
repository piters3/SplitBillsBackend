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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Bill>().ToTable("Bills");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Subcategory>().ToTable("Subcategories");

            builder.Entity<UserBill>()
                .HasKey(ub => new { ub.UserId, ub.BillId });

            builder.Entity<UserBill>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBills)
                .HasForeignKey(ub => ub.UserId);

            builder.Entity<UserBill>()
                .HasOne(ub => ub.Bill)
                .WithMany(b => b.UserBills)
                .HasForeignKey(ub => ub.BillId);

            base.OnModelCreating(builder);
        }
    }
}
