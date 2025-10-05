using EcommercePlatform.Core.Entities;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public interface ICartService
    {
        Task<ShoppingCart> GetCartAsync(string userId);
        Task ClearCartAsync(string userId);
        Task RemoveItemFromCartAsync(string userId, int cartItemId);
        Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);

        // --- تمت الإضافة لتفعيل وظيفة إضافة المنتج للسلة ---
        Task AddToCartAsync(string userId, int productId, int quantity);
    }
}

