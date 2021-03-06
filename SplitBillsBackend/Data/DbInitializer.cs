﻿using Microsoft.AspNetCore.Identity;
using SplitBillsBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace SplitBillsBackend.Data
{
    public static class DbInitializer
    {
        private static User _pioter, _mati, _piotrek, _user, _admin;
        private static List<Category> _categories;
        private static List<Subcategory> _rozrywka, _jedzenie, _dom, _zycie, _uzytkowanie, _transport;
        private static List<Bill> _bills;
        private static List<UserBill> _userbills;
        private static List<Note> _notes;


        public static void Initialize(SplitBillsDbContext context)
        {

            var exist = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
            if (!exist)
            {
                Console.WriteLine("Tworzenie bazy...");
                if (context.Database.EnsureCreated())
                {
                    Console.WriteLine("Dodawanie użytkowników i ról...");
                    AddUsersAndRoles(context);

                    Console.WriteLine("Dodawanie kategorii...");
                    AddCategories(context);

                    Console.WriteLine("Dodawanie podkategorii...");
                    AddSubcategories(context);

                    Console.WriteLine("Dodawanie rachunków...");
                    AddNotes(context);
                    AddBills(context);
                    AddUserBills(context);

                    Console.WriteLine("Dodawanie przyjaciół...");
                    AddFriends(context);

                    Console.WriteLine("Dodawanie historii...");
                    AddHistories(context);
                }
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

        private static void AddFriends(SplitBillsDbContext context)
        {
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

        private static void AddUserBills(SplitBillsDbContext context)
        {
            _userbills = new List<UserBill>
            {
                new UserBill { Bill = _bills[0], User = _pioter, Amount = 1.30m },
                new UserBill { Bill = _bills[0], User = _mati, Amount = 1.30m },

                new UserBill { Bill = _bills[1], User = _pioter, Amount = 23.14m },
                new UserBill { Bill = _bills[1], User = _mati, Amount = 23.14m },
                new UserBill { Bill = _bills[1], User = _piotrek, Amount = 23.14m },
                new UserBill { Bill = _bills[1], User = _user, Amount = 23.14m },

                new UserBill { Bill = _bills[2], User = _mati, Amount = 18.00m },
                new UserBill { Bill = _bills[2], User = _pioter, Amount = 18.00m },
                new UserBill { Bill = _bills[2], User = _user, Amount = 18.00m },

                new UserBill { Bill = _bills[3], User = _mati, Amount =  500.00m },
                new UserBill { Bill = _bills[3], User = _pioter, Amount = 500.00m },
                new UserBill { Bill = _bills[3], User = _piotrek, Amount = 500.00m },

                new UserBill { Bill = _bills[4], User = _pioter, Amount = 25.00m },
                new UserBill { Bill = _bills[4], User = _mati, Amount = 25.00m }
            };

            foreach (var b in _userbills)
            {
                if (!context.UserBills.Any(x => x.Bill == b.Bill))
                {
                    context.UserBills.Add(b);
                }
            }
            context.SaveChanges();
        }

        private static void AddBills(SplitBillsDbContext context)
        {
            _bills = new List<Bill>
            {
                new Bill
                {
                    Creator = _pioter,
                    TotalAmount = 2.60m,
                    Date = new DateTime(2018, 6, 1, 12, 23, 4),
                    Description = "Bułki",
                    Notes = { _notes[0], _notes[1] },
                    Subcategory = _jedzenie[1]
                },
                new Bill
                {
                    Creator = _pioter,
                    TotalAmount = 92.56m,
                    Date = new DateTime(2018, 4, 01, 12, 33, 14),
                    Description = "Jägermeister",
                    Notes = { _notes[2], _notes[3] },
                    Subcategory = _jedzenie[2]
                },
                new Bill
                {
                    Creator = _mati,
                    TotalAmount = 54.00m,
                    Date = new DateTime(2018, 6, 11, 10, 3, 49),
                    Description = "Kino",
                    Notes = { _notes[4], _notes[5] },
                    Subcategory = _rozrywka[1]
                },
                new Bill
                {
                    Creator = _piotrek,
                    TotalAmount = 1500.00m,
                    Date = new DateTime(2018, 5, 9, 10, 30, 0),
                    Description = "Czynsz maj",
                    Notes = { _notes[6], _notes[7] },
                    Subcategory = _dom[6]
                },
                new Bill
                {
                    Creator = _mati,
                    TotalAmount = 50.00m,
                    Date = new DateTime(2018, 5, 10, 11, 30, 0),
                    Description = "Obiad",
                    Notes = { _notes[8], _notes[9] },
                    Subcategory = _jedzenie[0]
                }
            };

            foreach (var b in _bills)
            {
                if (!context.Bills.Any(x => x.Description == b.Description))
                {
                    context.Bills.Add(b);
                    //context.Histories.Add(new History
                    //{
                    //    Bill = b,
                    //    Creator = b.Creator,
                    //    Date = b.Date,
                    //    Description = b.Description,
                    //    HistoryType = ActionType.Add
                    //});
                }
            }

            context.SaveChanges();
        }

        private static void AddSubcategories(SplitBillsDbContext context)
        {
            _rozrywka = new List<Subcategory>
            {
                new Subcategory {Name = "Gry", Category = _categories[0]},
                new Subcategory {Name = "Filmy", Category = _categories[0]},
                new Subcategory {Name = "Muzyka", Category = _categories[0]},
                new Subcategory {Name = "Sport", Category = _categories[0]},
                new Subcategory {Name = "Inne", Category = _categories[0]}
            };

            foreach (var s in _rozrywka)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();

            _jedzenie = new List<Subcategory>
            {
                new Subcategory {Name = "Jedzenie na mieście", Category = _categories[1]},
                new Subcategory {Name = "Artykuły spożywcze", Category = _categories[1]},
                new Subcategory {Name = "Trunek", Category = _categories[1]},
                new Subcategory {Name = "Inne", Category = _categories[1]}
            };

            foreach (var s in _jedzenie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();

            _dom = new List<Subcategory>
            {
                new Subcategory {Name = "Elektronika", Category = _categories[2]},
                new Subcategory {Name = "Meble", Category = _categories[2]},
                new Subcategory {Name = "Artykuły gospodarstwa domowego", Category = _categories[2]},
                new Subcategory {Name = "Konserwacja", Category = _categories[2]},
                new Subcategory {Name = "Hipoteka", Category = _categories[2]},
                new Subcategory {Name = "Zwierzaki", Category = _categories[2]},
                new Subcategory {Name = "Czynsz", Category = _categories[2]},
                new Subcategory {Name = "Usługi", Category = _categories[2]},
                new Subcategory {Name = "Inne", Category = _categories[2]}
            };

            foreach (var s in _dom)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();

            _zycie = new List<Subcategory>
            {
                new Subcategory {Name = "Odzież", Category = _categories[3]},
                new Subcategory {Name = "Prezenty", Category = _categories[3]},
                new Subcategory {Name = "Ubezpieczenie", Category = _categories[3]},
                new Subcategory {Name = "Medyczne wydatki", Category = _categories[3]},
                new Subcategory {Name = "Podatki", Category = _categories[3]},
                new Subcategory {Name = "Inne", Category = _categories[3]}
            };

            foreach (var s in _zycie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();

            _transport = new List<Subcategory>
            {
                new Subcategory {Name = "Rower", Category = _categories[4]},
                new Subcategory {Name = "Autobus/pociąg", Category = _categories[4]},
                new Subcategory {Name = "Samochód", Category = _categories[4]},
                new Subcategory {Name = "Benzyna/gaz", Category = _categories[4]},
                new Subcategory {Name = "Hotel", Category = _categories[4]},
                new Subcategory {Name = "Parking", Category = _categories[4]},
                new Subcategory {Name = "Samolot", Category = _categories[4]},
                new Subcategory {Name = "Taksówka", Category = _categories[4]},
                new Subcategory {Name = "Inne", Category = _categories[4]}
            };

            foreach (var s in _transport)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();

            _uzytkowanie = new List<Subcategory>
            {
                new Subcategory {Name = "Czyszczenie", Category = _categories[5]},
                new Subcategory {Name = "Elektryka", Category = _categories[5]},
                new Subcategory {Name = "Ogrzewanie", Category = _categories[5]},
                new Subcategory {Name = "Śmieci", Category = _categories[5]},
                new Subcategory {Name = "TV/Telefon/Internet", Category = _categories[5]},
                new Subcategory {Name = "Woda", Category = _categories[5]},
                new Subcategory {Name = "Inne", Category = _categories[5]}
            };

            foreach (var s in _uzytkowanie)
            {
                if (!context.Subcategories.Any(x => x.Name == s.Name))
                {
                    context.Subcategories.Add(s);
                }
            }

            context.SaveChanges();
        }

        private static void AddCategories(SplitBillsDbContext context)
        {
            _categories = new List<Category>
            {
                new Category {Name = "Rozrywka"},
                new Category {Name = "Jedzenie i picie"},
                new Category {Name = "Dom"},
                new Category {Name = "Życie"},
                new Category {Name = "Transport"},
                new Category {Name = "Użytkowanie"}
            };

            foreach (var c in _categories)
            {
                if (!context.Categories.Any(x => x.Name == c.Name))
                {
                    context.Categories.Add(c);
                }
            }

            context.SaveChanges();
        }

        private static void AddNotes(SplitBillsDbContext context)
        {
            _notes = new List<Note>
            {
                new Note {Text = "Jeden"},
                new Note {Text = "Dwa"},
                new Note {Text = "Trzy"},
                new Note {Text = "Cztery"},
                new Note {Text = "Pięć"},
                new Note {Text = "Sześć"},
                new Note {Text = "Siedem"},
                new Note {Text = "Osiem"},
                new Note {Text = "Dziewięć"},
                new Note {Text = "Dziesięć"}
            };

            foreach (var n in _notes)
            {
                if (!context.Notes.Any(x => x.Text == n.Text))
                {
                    context.Notes.Add(n);
                }
            }

            context.SaveChanges();
        }

        private static void AddHistories(SplitBillsDbContext context)
        {
            foreach (var b in context.Bills)
            {
                var history = new History
                {
                    Bill = b,
                    Creator = b.Creator,
                    Date = b.Date,
                    Description = b.Description,
                    HistoryType = ActionType.Add,
                };

                context.Histories.Add(history);

                foreach (var reader in b.UserBills)
                {
                    if (reader.User.Id != b.Creator.Id)
                    {
                        context.Notifications.Add(new Notification
                        {
                            History = history,
                            Readed = false,
                            Reader = reader.User
                        });
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
