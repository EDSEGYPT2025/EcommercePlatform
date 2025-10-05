using EcommercePlatform.Core.Entities;
using EcommercePlatform.Core.Enums;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, ICartService cartService, ILogger<OrderService> logger)
        {
            _context = context;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(string userId, string shippingAddress, string city)
        {
            var cart = await _cartService.GetCartAsync(userId);

            if (cart == null || !cart.Items.Any())
            {
                throw new InvalidOperationException("Shopping cart is empty.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending,
                    ShippingAddress = shippingAddress,
                    City = city,
                    Total = cart.Items.Sum(item => item.Quantity * item.Product.Price)
                };

                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new OrderItem
                    {
                        Order = order,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Product.Price
                    };

                    // The product should already be included from GetCartAsync
                    if (cartItem.Product.Stock < cartItem.Quantity)
                    {
                        await transaction.RollbackAsync();
                        throw new InvalidOperationException($"Not enough stock for product {cartItem.Product.Name}.");
                    }

                    cartItem.Product.Stock -= cartItem.Quantity;
                    _context.Products.Update(cartItem.Product);
                    order.OrderItems.Add(orderItem);
                }

                _context.Orders.Add(order);

                await _cartService.ClearCartAsync(userId);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Order {OrderId} created successfully for user {UserId}", order.Id, userId);
                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating an order for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _context.Orders
               .Where(o => o.UserId == userId)
               .Include(o => o.OrderItems)
               .ThenInclude(oi => oi.Product)
               .OrderByDescending(o => o.OrderDate)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Status for Order {OrderId} updated to {Status}", orderId, status);
            }
            else
            {
                _logger.LogWarning("Attempted to update status for a non-existent order {OrderId}", orderId);
            }
        }
    }
}

