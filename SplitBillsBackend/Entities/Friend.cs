namespace SplitBillsBackend.Entities
{
    public class Friend
    {
        public int Id { get; set; }
        public virtual User UserFriend { get; set; }
        //public string FirstFriendId { get; set; }
        //public virtual User FirstFriend { get; set; }
        //public string SecondFriendId { get; set; }
        //public virtual User SecondFriend { get; set; }
    }
}
