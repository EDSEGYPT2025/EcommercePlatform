// Category.cs
namespace EcommercePlatform.Core.Entities
{
    // Category.cs - التصنيفات
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}