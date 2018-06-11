namespace SplitBillsBackend.Entities
{
    public class UserBill
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
