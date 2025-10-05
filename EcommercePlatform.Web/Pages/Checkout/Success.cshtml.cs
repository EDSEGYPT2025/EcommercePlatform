// Pages/Checkout/Success.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;

namespace EcommercePlatform.Web.Pages.Checkout
{
    public class SuccessModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IStoreService _storeService;

        public SuccessModel(IOrderService orderService, IStoreService storeService)
        {
            _orderService = orderService;
            _storeService = storeService;
        }

        public Order Order { get; set; }
        public Store Store { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId, string storeSlug)
        {
            Order = await _orderService.GetOrderByIdAsync(orderId);
            Store = await _storeService.GetStoreBySlugAsync(storeSlug);

            if (Order == null || Store == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}