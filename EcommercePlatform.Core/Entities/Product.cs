// Product.cs
namespace EcommercePlatform.Core.Entities
{
    // Product.cs - المنتجات
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int Stock { get; set; }
        public string MainImage { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}