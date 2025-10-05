// ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace EcommercePlatform.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Store> Stores { get; set; }
    }

    // Enums
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}