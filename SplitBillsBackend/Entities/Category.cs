using System.Collections.Generic;

namespace SplitBillsBackend.Entities
{
    public class Category
    {
        public Category()
        {
            Subcategories = new List<Subcategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Subcategory> Subcategories { get; set; }        
    }
}
