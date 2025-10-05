// SubscriptionPlan.cs
namespace EcommercePlatform.Core.Entities
{
    // SubscriptionPlan.cs - خطط الاشتراك
    public class SubscriptionPlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal YearlyPrice { get; set; }
        public int MaxProducts { get; set; }
        public int MaxOrders { get; set; }
        public bool HasCustomDomain { get; set; }
        public bool HasAdvancedReports { get; set; }
        public ICollection<Store> Stores { get; set; }
    }
}