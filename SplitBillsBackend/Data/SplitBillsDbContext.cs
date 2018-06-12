using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;
using System.Linq;

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
        public DbSet<UserBill> UserBills { get; set; }
        public DbSet<Friend> Friends { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Bill>().ToTable("Bills");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Subcategory>().ToTable("Subcategories");
            builder.Entity<Friend>().ToTable("Friends");

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

            foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties()).Where(p => p.ClrType == typeof(decimal)))
            {
                property.Relational().ColumnType = "decimal(7, 2)";
            }

            //builder.Entity<Friend>()
            //    .HasKey(e => new { e.FirstFriendId, e.SecondFriendId });

            //builder.Entity<Friend>()
            //    .HasOne(e => e.FirstFriend)
            //    .WithMany(e => e.Friends)
            //    .HasForeignKey(e => e.FirstFriendId);

            //builder.Entity<Friend>()
            //        .HasOne(e => e.SecondFriend)
            //    .WithMany(e => e.OherFriends)
            //    .HasForeignKey(e => e.SecondFriendId);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
