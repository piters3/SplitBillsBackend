using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public class User: IdentityUser
    {
        public User()
        {
            UserBills = new List<UserBill>();
        }

        public string Name { get; set; }

        //public virtual List<Bill> Bills { get; set; }
        public List<UserBill> UserBills { get; set; }
    }
}
