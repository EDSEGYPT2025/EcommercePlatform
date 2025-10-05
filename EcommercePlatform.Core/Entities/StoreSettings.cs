// StoreSettings.cs
namespace EcommercePlatform.Core.Entities
{
    // StoreSettings.cs - إعدادات المتجر
    public class StoreSettings
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }

        public string Currency { get; set; } = "EGP";
        public string Language { get; set; } = "ar";
        public string PrimaryColor { get; set; } = "#000000";
        public string SecondaryColor { get; set; } = "#ffffff";

        // إعدادات الشحن
        public decimal ShippingFee { get; set; }
        public bool FreeShippingEnabled { get; set; }
        public decimal FreeShippingThreshold { get; set; }

        // معلومات الاتصال
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}