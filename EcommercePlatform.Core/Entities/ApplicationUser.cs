using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;

namespace EcommercePlatform.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Store> Stores { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        // --- تمت الإضافة لإنشاء علاقة واحد-لواحد مع المتجر ---
        public Store Store { get; set; }
    }
}

