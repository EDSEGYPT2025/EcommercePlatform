// Pages/Checkout/Index.cshtml.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Web.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IStoreService _storeService;
        private readonly IOrderService _orderService;

        public IndexModel(ICartService cartService, IStoreService storeService, IOrderService orderService)
        {
            _cartService = cartService;
            _storeService = storeService;
            _orderService = orderService;
        }

        public ShoppingCart Cart { get; set; }
        public Store Store { get; set; }

        [BindProperty]
        public OrderCheckoutModel OrderModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StoreSlug { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = _cartService.GetCart();

            if (!Cart.Items.Any())
            {
                return RedirectToPage("/Cart/Index", new { storeSlug = StoreSlug });
            }

            Store = await _storeService.GetStoreBySlugAsync(StoreSlug);
            if (Store == null)
            {
                return NotFound();
            }

            // حساب رسوم الشحن
            Cart.ShippingFee = Store.Settings.ShippingFee;

            if (Store.Settings.FreeShippingEnabled && Cart.SubTotal >= Store.Settings.FreeShippingThreshold)
            {
                Cart.ShippingFee = 0;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Cart = _cartService.GetCart();
            Store = await _storeService.GetStoreBySlugAsync(StoreSlug);

            if (!ModelState.IsValid || !Cart.Items.Any())
            {
                return Page();
            }

            // إنشاء الطلب
            var order = new Order
            {
                StoreId = Store.Id,
                CustomerName = OrderModel.CustomerName,
                CustomerEmail = OrderModel.CustomerEmail,
                CustomerPhone = OrderModel.CustomerPhone,
                ShippingAddress = OrderModel.ShippingAddress,
                City = OrderModel.City,
                Notes = OrderModel.Notes,
                SubTotal = Cart.SubTotal,
                ShippingFee = Cart.ShippingFee,
                Total = Cart.Total,
                Status = OrderStatus.Pending,
                OrderItems = Cart.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    TotalPrice = item.Total
                }).ToList()
            };

            var createdOrder = await _orderService.CreateOrderAsync(order);
            _cartService.ClearCart();

            TempData["Success"] = "تم إرسال طلبك بنجاح!";
            return RedirectToPage("/Checkout/Success", new { orderId = createdOrder.Id, storeSlug = StoreSlug });
        }
    }

    public class OrderCheckoutModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public string City { get; set; }

        public string Notes { get; set; }
    }
}