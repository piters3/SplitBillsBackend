using System.Collections.Generic;

namespace SplitBillsBackend.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubcategoryModel> Subcategories { get; set; }
    }
}
