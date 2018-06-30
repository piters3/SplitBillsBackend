namespace SplitBillsBackend.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual Bill Bill { get; set; }
    }
}
