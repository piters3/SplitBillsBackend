using Microsoft.AspNetCore.Identity;
using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitBillsBackend.Data
{
    public static class DbInitializer
    {
        private static User _pioter;
        private static User _mati;
        private static User _piotrek;
        private static User _user;
        private static User _admin;


        public static void Initialize(SplitBillsDbContext context)
        {
            if (context.Database.EnsureCreated())
            {
                AddUsersAndRoles(context);

                AddData(context);
            }
        }

        private static void AddUsersAndRoles(SplitBillsDbContext context)
        {
            var adminRole = new Role { Name = "admin", NormalizedName = "ADMIN" };
            var userRole = new Role { Name = "user", NormalizedName = "USER" };

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
                PhoneNumber = "111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "Admin",
                Surname = "Adminowski",
                Enabled = true,
                RegisterDate = new DateTime(2018, 6, 1, 12, 23, 4)
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
                PhoneNumber = "987426373",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "User",
                Surname = "Userowski",
                Enabled = true,
                RegisterDate = new DateTime(2017, 5, 4, 10, 20, 0)
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "user");
                user.PasswordHash = hashed;
                context.Users.Add(user);
            }
            context.SaveChanges();

            var pioter = new User
            {
                Email = "pioter@example.com",
                NormalizedEmail = "PIOTER@EXAMPLE.COM",
                UserName = "pioter",
                NormalizedUserName = "PIOTER",
                PhoneNumber = "517906254",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "Piotr",
                Surname = "Strzelecki",
                Enabled = true,
                RegisterDate = new DateTime(2018, 1, 1, 1, 2, 1)
            };

            if (!context.Users.Any(u => u.UserName == pioter.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(pioter, "pioter");
                pioter.PasswordHash = hashed;
                context.Users.Add(pioter);
            }
            context.SaveChanges();

            var mati = new User
            {
                Email = "mati@example.com",
                NormalizedEmail = "MATI@EXAMLE.COM",
                UserName = "mati",
                NormalizedUserName = "MATI",
                PhoneNumber = "798654678",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "Mateusz",
                Surname = "Szpinda",
                Enabled = true,
                RegisterDate = new DateTime(2017, 12, 12, 12, 22, 20)
            };

            if (!context.Users.Any(u => u.UserName == mati.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(mati, "mati");
                mati.PasswordHash = hashed;
                context.Users.Add(mati);
            }
            context.SaveChanges();

            var piotrek = new User
            {
                Email = "piotrek@example.com",
                NormalizedEmail = "PIOTREK@EXAMPLE.COM",
                UserName = "piotrek",
                NormalizedUserName = "PIOTREK",
                PhoneNumber = "798647836",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "Piotr",
                Surname = "Sobiborowicz",
                Enabled = true,
                RegisterDate = new DateTime(2017, 9, 4, 10, 14, 15)
            };

            if (!context.Users.Any(u => u.UserName == piotrek.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(piotrek, "piotrek");
                piotrek.PasswordHash = hashed;
                context.Users.Add(piotrek);
            }
            context.SaveChanges();

            var adminUserRole = new UserRole
            {
                UserId = context.Users.Single(r => r.UserName == admin.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == adminRole.Name).Id
            };
            context.UserRoles.Add(adminUserRole);

            var userUserRole = new UserRole
            {
                UserId = context.Users.Single(r => r.UserName == user.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == userRole.Name).Id
            };
            context.UserRoles.Add(userUserRole);

            var pioterAdminRole = new UserRole
            {
                UserId = context.Users.Single(r => r.UserName == pioter.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == adminRole.Name).Id
            };
            context.UserRoles.Add(pioterAdminRole);

            var matiUserRole = new UserRole
            {
                UserId = context.Users.Single(r => r.UserName == mati.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == userRole.Name).Id
            };
            context.UserRoles.Add(matiUserRole);

            context.SaveChanges();

            var piotrekUserRole = new UserRole
            {
                UserId = context.Users.Single(r => r.UserName == piotrek.UserName).Id,
                RoleId = context.Roles.Single(r => r.Name == userRole.Name).Id
            };
            context.UserRoles.Add(piotrekUserRole);

            _pioter = pioter;
            _mati = mati;
            _piotrek = piotrek;
            _user = user;
            _admin = admin;

            context.SaveChanges();
        }

        private static void AddData(SplitBillsDbContext context)
        {
            var categories = new List<Category>
            {
                new Category { Name = "Rozrywka" },
                new Category { Name = "Jedzenie i picie" },
                new Category { Name = "Dom" },
                new Category { Name = "Życie" },
                new Category { Name = "Transport" },
                new Category { Name = "Użytkowanie" }
            };

            foreach (var c in categories)
            {
                if (!context.Categories.Any(x => x.Name == c.Name))
                {
                    context.Categories.Add(c);
                }
            }
            context.SaveChanges();


            var rozrywka = new List<Subcategory>
            {
                new Subcategory { Name = "Gry", Category = categories[0] },
                new Subcategory { Name = "Filmy", Category = categories[0] },
                new Subcategory { Name = "Muzyka", Category = categories[0] },
                new Subcategory { Name = "Sport", Category = categories[0] },
                new Subcategory { Name = "Inne", Category = categories[0] }
            };

            foreach (var s in rozrywka)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var jedzenie = new List<Subcategory>
            {
                new Subcategory { Name = "Jedzenie na mieście", Category = categories[1] },
                new Subcategory { Name = "Artykuły spożywcze", Category = categories[1] },
                new Subcategory { Name = "Trunek", Category = categories[1] },
                new Subcategory { Name = "Inne", Category = categories[1] }
            };

            foreach (var s in jedzenie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var dom = new List<Subcategory>
            {
                new Subcategory { Name = "Elektronika", Category = categories[2] },
                new Subcategory { Name = "Meble", Category = categories[2] },
                new Subcategory { Name = "Artykuły gospodarstwa domowego", Category = categories[2] },
                new Subcategory { Name = "Konserwacja", Category = categories[2] },
                new Subcategory { Name = "Hipoteka", Category = categories[2] },
                new Subcategory { Name = "Zwierzaki", Category = categories[2] },
                new Subcategory { Name = "Czynsz", Category = categories[2] },
                new Subcategory { Name = "Usługi", Category = categories[2] },
                new Subcategory { Name = "Inne", Category = categories[2] }
            };

            foreach (var s in dom)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var zycie = new List<Subcategory>
            {
                new Subcategory { Name = "Odzież", Category = categories[3] },
                new Subcategory { Name = "Prezenty", Category = categories[3] },
                new Subcategory { Name = "Ubezpieczenie", Category = categories[3] },
                new Subcategory { Name = "Medyczne wydatki", Category = categories[3] },
                new Subcategory { Name = "Podatki", Category = categories[3] },
                new Subcategory { Name = "Inne", Category = categories[3] }
            };

            foreach (var s in zycie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var transport = new List<Subcategory>
            {
                new Subcategory { Name = "Rower", Category = categories[4] },
                new Subcategory { Name = "Autobus/pociąg", Category = categories[4] },
                new Subcategory { Name = "Samochód", Category = categories[4] },
                new Subcategory { Name = "Benzyna/gaz", Category = categories[4] },
                new Subcategory { Name = "Hotel", Category = categories[4] },
                new Subcategory { Name = "Parking", Category = categories[4] },
                new Subcategory { Name = "Samolot", Category = categories[4] },
                new Subcategory { Name = "Taksówka", Category = categories[4] },
                new Subcategory { Name = "Inne", Category = categories[4] }
            };

            foreach (var s in transport)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var uzytkowanie = new List<Subcategory>
            {
                new Subcategory { Name = "Czyszczenie", Category = categories[5] },
                new Subcategory { Name = "Elektryka", Category = categories[5] },
                new Subcategory { Name = "Ogrzewanie", Category = categories[5] },
                new Subcategory { Name = "Śmieci", Category = categories[5] },
                new Subcategory { Name = "TV/Telefon/Internet", Category = categories[5] },
                new Subcategory { Name = "Woda", Category = categories[5] },
                new Subcategory { Name = "Inne", Category = categories[5] }
            };

            foreach (var s in uzytkowanie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }
            context.SaveChanges();

            var bills = new List<Bill>
            {
                new Bill { Creator = _pioter, TotalAmount = 2.60m, Date = new DateTime(2018, 6, 1, 12, 23, 4), Description = "Bułki", Notes = "Dobry były", Subcategory = jedzenie[1] },
                new Bill { Creator = _pioter, TotalAmount = 92.56m, Date = new DateTime(2018, 4, 01, 12, 33, 14), Description = "Jägermeister", Notes = "Na melanż", Subcategory = jedzenie[2] },
                new Bill { Creator = _mati, TotalAmount = 54.00m, Date = new DateTime(2018, 6, 11, 10, 3, 49), Description = "Kino", Notes = "Kino notatki", Subcategory = rozrywka[1] },
                new Bill { Creator = _piotrek, TotalAmount = 1500.00m, Date = new DateTime(2018, 5, 9, 10, 30, 0), Description = "Czynsz maj", Notes = "Brak", Subcategory = dom[6] },
                new Bill { Creator = _mati, TotalAmount = 50.00m, Date = new DateTime(2018, 5, 10, 11, 30, 0), Description = "Obiad", Notes = "Nie ma notatek", Subcategory = jedzenie[0] }
            };

            foreach (var b in bills)
            {
                if (!context.Bills.Any(x => x.Description == b.Description))
                {
                    context.Bills.Add(b);
                    context.History.Add(new History
                    {
                        Bill = b,
                        Creator = b.Creator,
                        Date = b.Date,
                        Description = b.Description,
                        HistoryType = ActionType.Add
                    });
                }
            }
            context.SaveChanges();


            var userbills = new List<UserBill>
            {
                new UserBill { Bill = bills[0], User = _pioter, Amount = 1.30m },
                new UserBill { Bill = bills[0], User = _mati, Amount = 1.30m },

                new UserBill { Bill = bills[1], User = _pioter, Amount = 23.14m },
                new UserBill { Bill = bills[1], User = _mati, Amount = 23.14m },
                new UserBill { Bill = bills[1], User = _piotrek, Amount = 23.14m },
                new UserBill { Bill = bills[1], User = _user, Amount = 23.14m },

                new UserBill { Bill = bills[2], User = _mati, Amount = 18.00m },
                new UserBill { Bill = bills[2], User = _pioter, Amount = 18.00m },
                new UserBill { Bill = bills[2], User = _user, Amount = 18.00m },

                new UserBill { Bill = bills[3], User = _mati, Amount =  500.00m },
                new UserBill { Bill = bills[3], User = _pioter, Amount = 500.00m },
                new UserBill { Bill = bills[3], User = _piotrek, Amount = 500.00m },

                new UserBill { Bill = bills[4], User = _pioter, Amount = 25.00m },
                new UserBill { Bill = bills[4], User = _mati, Amount = 25.00m }
            };

            foreach (var b in userbills)
            {
                if (!context.UserBills.Any(x => x.Bill == b.Bill))
                {
                    context.UserBills.Add(b);
                }
            }
            context.SaveChanges();

            if (!context.Friends.Any())
            {
                _pioter.Friends.Add(new Friend { FirstFriend = _pioter, SecondFriend = _mati });
                _mati.Friends.Add(new Friend { FirstFriend = _mati, SecondFriend = _pioter });

                _pioter.Friends.Add(new Friend { FirstFriend = _pioter, SecondFriend = _piotrek });
                _piotrek.Friends.Add(new Friend { FirstFriend = _piotrek, SecondFriend = _pioter });

                _pioter.Friends.Add(new Friend { FirstFriend = _pioter, SecondFriend = _user });
                _user.Friends.Add(new Friend { FirstFriend = _user, SecondFriend = _pioter });

                _mati.Friends.Add(new Friend { FirstFriend = _mati, SecondFriend = _piotrek });
                _piotrek.Friends.Add(new Friend { FirstFriend = _piotrek, SecondFriend = _mati });

                _pioter.Friends.Add(new Friend { FirstFriend = _pioter, SecondFriend = _admin });
                _admin.Friends.Add(new Friend { FirstFriend = _admin, SecondFriend = _pioter });
            }
            context.SaveChanges();
        }
    }
}
