using System;
using System.Collections.Generic;

namespace EcommercePlatform.Core.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; } // e.g., "my-awesome-store"
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string LogoUrl { get; set; }

        // --- تمت الإضافة ---
        public bool IsActive { get; set; } = true; // Store is active by default
        public int? SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }

        // --- تم التعديل لتصبح الأسماء أكثر وضوحًا ---
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public StoreSettings Settings { get; set; }

    }
}

