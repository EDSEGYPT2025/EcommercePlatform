// IOrderService.cs
using EcommercePlatform.Core.Entities;

namespace EcommercePlatform.Services
{
    // IOrderService.cs
    public interface IOrderService
    {
        Task<List<Order>> GetStoreOrdersAsync(int storeId);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<Order> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
