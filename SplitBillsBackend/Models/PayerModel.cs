namespace SplitBillsBackend.Models
{
    public class PayerModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public bool Settled { get; set; }
    }
}
