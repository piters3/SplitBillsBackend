using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SplitBillsBackend.Entities;
using System.Linq;

namespace SplitBillsBackend.Data
{
    public class SplitBillsDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
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
            base.OnModelCreating(builder);

            builder.Entity<Bill>().ToTable("Bills");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Subcategory>().ToTable("Subcategories");
            builder.Entity<Friend>().ToTable("Friends");


            builder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasMany(u => u.UserRoles)
                 .WithOne(ur => ur.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            });

            builder.Entity<Role>(role =>
            {
                role.ToTable("Roles");
                role.HasKey(r => r.Id);
                role.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
                role.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                role.Property(u => u.Name).HasMaxLength(256);
                role.Property(u => u.NormalizedName).HasMaxLength(256);

                role.HasMany<UserRole>()
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
                role.HasMany<IdentityRoleClaim<int>>()
                    .WithOne()
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<IdentityRoleClaim<int>>(roleClaim =>
            {
                roleClaim.HasKey(rc => rc.Id);
                roleClaim.ToTable("RoleClaims");
            });

            builder.Entity<UserRole>(userRole =>
            {
                userRole.ToTable("UserRoles");
                userRole.HasKey(r => new { r.UserId, r.RoleId });
            });

            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");



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

            builder.Entity<Friend>()
                .HasOne(e => e.FirstFriend)
                .WithMany(e => e.Friends)
                //.HasForeignKey(e => e.FirstFriendId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
            .HasOne(e => e.SecondFriend)
            .WithMany(e => e.OtherFriends)
            //.HasForeignKey(e => e.SecondFriendId)
            .OnDelete(DeleteBehavior.Restrict);        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
