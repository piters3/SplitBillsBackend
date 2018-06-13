using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public class User : IdentityUser
    {
        public User()
        {
            UserBills = new List<UserBill>();
            Friends = new List<Friend>();
            OherFriends = new List<Friend>();
        }

        public string Name { get; set; }

        public virtual List<UserBill> UserBills { get; set; }
        public virtual List<Friend> Friends { get; set; }
        public virtual List<Friend> OherFriends { get; set; }
    }
}
