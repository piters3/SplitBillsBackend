﻿using System.Collections.Generic;

namespace SplitBillsBackend.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<FriendModel> Friends { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<BillModel> Bills { get; set; }
    }
}