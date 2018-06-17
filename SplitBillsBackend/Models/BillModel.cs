using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Models
{
    public class BillModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public FriendModel Creator { get; set; }
        public List<PayerModel> Payers { get; set; }
    }
}
