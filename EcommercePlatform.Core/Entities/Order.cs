// Order.cs
namespace EcommercePlatform.Core.Entities
{
    // Order.cs - الطلبات
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }

        // معلومات العميل
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string City { get; set; }
        public string Notes { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}