namespace SplitBillsBackend.Models
{
    public class BorrowerModel
    {
        public int Id { get; set; }     
        public string Name { get; set; }
        public string SurName { get; set; }
        public decimal Amount { get; set; }
    }
}
