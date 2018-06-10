using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
