using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public partial class Role : IdentityRole<string>
    { }

    public partial class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class User : IdentityUser<string>
    {
        public User()
        {
            UserBills = new List<UserBill>();
            Friends = new List<Friend>();
            OtherFriends = new List<Friend>();
            UserRoles = new List<UserRole>();
        }

        public string Name { get; set; }

        public virtual List<UserBill> UserBills { get; set; }
        public virtual List<Friend> Friends { get; set; }
        public virtual List<Friend> OtherFriends { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
    }
}
