using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EcommercePlatform.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- تمت الإضافة ---
        public bool IsActive { get; set; } = true; // Product is active by default
        public decimal? CompareAtPrice { get; set; } // Nullable for products not on sale

        [NotMapped] // This property is not saved to the database
        public string MainImage => Images?.FirstOrDefault()?.Url ?? "/images/placeholder.png";
    }
}

