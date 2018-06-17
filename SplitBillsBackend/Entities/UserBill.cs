namespace SplitBillsBackend.Entities
{
    public class UserBill
    {
        public UserBill()
        {
            Settled = false;
        }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }
        public decimal Amount { get; set; }
        public bool Settled { get; set; }
    }
}
