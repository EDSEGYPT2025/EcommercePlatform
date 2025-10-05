// ======================================
// OrderService.cs
// ======================================
using Microsoft.EntityFrameworkCore;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;

namespace EcommercePlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;

        public OrderService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public async Task<List<Order>> GetStoreOrdersAsync(int storeId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.StoreId == storeId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> GetOrderByNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            // توليد رقم طلب فريد
            order.OrderNumber = await GenerateUniqueOrderNumberAsync();
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;

            // حساب المجاميع
            order.SubTotal = order.OrderItems.Sum(item => item.TotalPrice);
            order.Total = order.SubTotal + order.ShippingFee;

            _context.Orders.Add(order);

            // تحديث المخزون
            foreach (var item in order.OrderItems)
            {
                await _productService.UpdateStockAsync(item.ProductId, item.Quantity);
            }

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(int storeId, OrderStatus status)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.StoreId == storeId && o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesAsync(int storeId)
        {
            return await _context.Orders
                .Where(o => o.StoreId == storeId &&
                       (o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Confirmed))
                .SumAsync(o => o.Total);
        }

        public async Task<int> GetTotalOrdersCountAsync(int storeId)
        {
            return await _context.Orders
                .Where(o => o.StoreId == storeId)
                .CountAsync();
        }

        private async Task<string> GenerateUniqueOrderNumberAsync()
        {
            string orderNumber;
            do
            {
                orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
            }
            while (await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber));

            return orderNumber;
        }
    }
}