namespace SplitBillsBackend.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public bool Readed { get; set; }

        public virtual User Reader { get; set; }
        public virtual History History { get; set; }
    }
}
