using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Models
{
    public class AddBillPayerModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }

    public class AddBillModel
    {
        public AddBillModel()
        {
             Payers = new List<AddBillPayerModel>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public int SubcategoryId { get; set; }
        public int CreatorId { get; set; }
        public List<AddBillPayerModel> Payers { get; set; }
    }
}
