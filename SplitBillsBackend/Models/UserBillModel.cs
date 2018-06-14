using System;

namespace SplitBillsBackend.Models
{
    public class UserBillModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
    }
}
