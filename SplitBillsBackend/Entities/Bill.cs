﻿using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public class Bill
    {
        public Bill()
        {
            UserBills = new List<UserBill>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public User Creator { get; set; }

        public virtual List<UserBill> UserBills { get; set; }
        public virtual Subcategory Subcategory { get; set; }
    }
}
