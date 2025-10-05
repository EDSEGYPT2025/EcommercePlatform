// Store.cs
namespace EcommercePlatform.Core.Entities
{
    // Store.cs - المتجر
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; } // للرابط الفريد
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Domain { get; set; } // نطاق فرعي مخصص
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubscriptionEndDate { get; set; }

        // علاقات
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Order> Orders { get; set; }
        public StoreSettings Settings { get; set; }
    }
}