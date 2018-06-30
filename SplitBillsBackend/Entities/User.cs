using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public partial class Role : IdentityRole<int>
    { }

    public partial class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class User : IdentityUser<int>
    {
        public User()
        {
            UserBills = new List<UserBill>();
            Friends = new List<Friend>();
            OtherFriends = new List<Friend>();
            UserRoles = new List<UserRole>();
            Bills = new List<Bill>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Enabled { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ConnectionId { get; set; }
        public bool Connected { get; set; }

        public virtual List<Bill> Bills { get; set; }
        public virtual List<UserBill> UserBills { get; set; }
        public virtual List<Friend> Friends { get; set; }
        public virtual List<Friend> OtherFriends { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
