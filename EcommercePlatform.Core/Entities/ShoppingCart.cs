using System;
using System.Collections.Generic;

namespace EcommercePlatform.Core.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreatedAt { get; set; } // <-- هذا هو السطر المطلوب
    }
}

