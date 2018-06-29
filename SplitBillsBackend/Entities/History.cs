using System;
using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public enum ActionType
    {
        Add,
        Edit,
        Update,
        Delete,
        Comment,
        Paid
    }

    public class History
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ActionType HistoryType { get; set; }
        public string Description { get; set; }

        public virtual User Creator { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
