using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public class Subcategory
    {
        public Subcategory()
        {
            Bills = new List<Bill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
