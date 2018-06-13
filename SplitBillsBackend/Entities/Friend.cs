namespace SplitBillsBackend.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public virtual User FirstFriend { get; set; }
        public virtual User SecondFriend { get; set; }
    }
}
