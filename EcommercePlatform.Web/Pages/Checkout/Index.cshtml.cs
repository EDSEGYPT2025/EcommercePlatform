using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace EcommercePlatform.Web.Pages.Checkout
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IStoreService _storeService;
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ICartService cartService,
            IStoreService storeService,
            IOrderService orderService,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger)
        {
            _cartService = cartService;
            _storeService = storeService;
            _orderService = orderService;
            _userManager = userManager;
            _logger = logger;
        }

        public ShoppingCart Cart { get; set; }
        public Store Store { get; set; }

        [BindProperty]
        public OrderInputModel OrderModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StoreSlug { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Challenge();

            Cart = await _cartService.GetCartAsync(userId);
            Store = await _storeService.GetStoreBySlugAsync(StoreSlug);

            if (Cart == null || Cart.Items.Count == 0)
            {
                TempData["Error"] = "سلة التسوق فارغة.";
                return RedirectToPage("/Index");
            }

            if (Store == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            OrderModel = new OrderInputModel
            {
                CustomerName = user.FullName,
                CustomerEmail = user.Email,
                CustomerPhone = user.PhoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // إذا كان النموذج غير صالح، نحتاج إلى إعادة تحميل بيانات الصفحة
                var userId = _userManager.GetUserId(User);
                Cart = await _cartService.GetCartAsync(userId);
                Store = await _storeService.GetStoreBySlugAsync(StoreSlug);
                return Page();
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                var order = await _orderService.CreateOrderAsync(userId, OrderModel.ShippingAddress, OrderModel.City);

                if (order == null)
                {
                    ModelState.AddModelError(string.Empty, "حدث خطأ أثناء إنشاء الطلب.");
                    return await OnGetAsync(); // إعادة تحميل الصفحة مع البيانات
                }

                return RedirectToPage("Success", new { orderId = order.Id, storeSlug = StoreSlug });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during checkout for user {UserId}", _userManager.GetUserId(User));
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return await OnGetAsync(); // إعادة تحميل الصفحة مع البيانات
            }
        }

        public class OrderInputModel
        {
            [Required(ErrorMessage = "الاسم الكامل مطلوب")]
            public string CustomerName { get; set; }

            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress]
            public string CustomerEmail { get; set; }

            [Required(ErrorMessage = "رقم الهاتف مطلوب")]
            public string CustomerPhone { get; set; }

            [Required(ErrorMessage = "عنوان الشحن مطلوب")]
            public string ShippingAddress { get; set; }

            [Required(ErrorMessage = "المدينة مطلوبة")]
            public string City { get; set; }
            public string Notes { get; set; }
        }
    }
}

