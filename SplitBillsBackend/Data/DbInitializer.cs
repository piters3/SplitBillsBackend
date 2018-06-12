using Microsoft.AspNetCore.Identity;
using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitBillsBackend.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SplitBillsDbContext context)
        {
            context.Database.EnsureCreated();

            AddUsersAndRoles(context);

            AddData(context);
        }

        private static void AddUsersAndRoles(SplitBillsDbContext context)
        {
            var adminRole = new IdentityRole { Name = "admin", NormalizedName = "ADMIN" };
            var userRole = new IdentityRole { Name = "user", NormalizedName = "USER" };

            if (!context.Roles.Any(r => r.Name == adminRole.Name))
            {
                context.Roles.Add(adminRole);
            }

            if (!context.Roles.Any(r => r.Name == userRole.Name))
            {
                context.Roles.Add(userRole);
            }

            var admin = new User
            {
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == admin.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(admin, "admin");
                admin.PasswordHash = hashed;
                context.Users.Add(admin);
            }

            var user = new User
            {
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                UserName = "user",
                NormalizedUserName = "USER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "user");
                user.PasswordHash = hashed;
                context.Users.Add(user);
            }
            context.SaveChanges();

            var adminUserRole = new IdentityUserRole<string>
            {
                UserId = context.Users.Single(r => r.UserName == admin.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == adminRole.Name).Id
            };
            context.UserRoles.Add(adminUserRole);

            var userUserRole = new IdentityUserRole<string>
            {
                UserId = context.Users.Single(r => r.UserName == user.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == userRole.Name).Id
            };
            context.UserRoles.Add(userUserRole);

            context.SaveChanges();
        }

        private static void AddData(SplitBillsDbContext context)
        {
            var categories = new List<Category>
            {
                new Category() { Name = "Entertainment" },
                new Category() { Name = "Food and drink" }
            };

            foreach (var c in categories)
            {
                if (!context.Categories.Any(x => x.Name == c.Name))
                {
                    context.Categories.Add(c);
                }
            }
            context.SaveChanges();


            var entertainment = new List<Subcategory>
            {
                new Subcategory{ Name = "Games", Category = categories[0] },
                new Subcategory{ Name = "Movies", Category = categories[0] },
                new Subcategory{ Name = "Music", Category = categories[0] },
                new Subcategory{ Name = "Other", Category = categories[0] },
                new Subcategory{ Name = "Sports", Category = categories[0] },
            };

            foreach (var s in entertainment)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var food = new List<Subcategory>
            {
                new Subcategory{ Name = "Dining out", Category = categories[1] },
                new Subcategory{ Name = "Groceries", Category = categories[1] },
                new Subcategory{ Name = "Liquor", Category = categories[1] },
                new Subcategory{ Name = "Other", Category = categories[1] }
            };

            foreach (var s in food)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var bills = new List<Bill>
            {
                new Bill{ Amount = 100.2m, Date = DateTime.Now, Description = "Bułki", Notes = "Notatki" },
                new Bill{ Amount = 23.76m, Date = DateTime.Now, Description = "Kino", Notes = "Kino notatki" }
            };

            foreach (var b in bills)
            {
                if (!context.Bills.Any(x => x.Description == b.Description))
                {
                    context.Bills.Add(b);
                }
            }
            context.SaveChanges();

            var userbills = new List<UserBill>
            {
                new UserBill{ Bill = bills[0], User = context.Users.ToList()[0] },
                new UserBill{ Bill = bills[1], User = context.Users.ToList()[1] }
            };

            foreach (var b in userbills)
            {
                if (!context.UserBills.Any(x => x.Bill == b.Bill))
                {
                    context.UserBills.Add(b);
                }
            }
            context.SaveChanges();

            context.Users.ToList()[0].Friends.Add(new Friend { UserFriend = context.Users.ToList()[1] });

            context.SaveChanges();

            //var friends = new List<Friend>
            //{
            //    new Friend{FirstFriend = context.Users.ToList()[0], SecondFriend = context.Users.ToList()[1]}
            //};

            //foreach (var b in friends)
            //{
            //    if (!context.Friends.Any(x => x.FirstFriend == b.FirstFriend))
            //    {
            //        context.Friends.Add(b);
            //    }
            //}
            //context.SaveChanges();
        }
    }
}
