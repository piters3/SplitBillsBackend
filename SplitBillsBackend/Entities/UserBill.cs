namespace SplitBillsBackend.Entities
{
    public class UserBill
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
