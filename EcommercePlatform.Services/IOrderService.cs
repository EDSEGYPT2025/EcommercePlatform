using EcommercePlatform.Core.Entities;
using EcommercePlatform.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public interface IOrderService
    {
        // تم تحديث التعريف ليقبل المدينة
        Task<Order> CreateOrderAsync(string userId, string shippingAddress, string city);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}

