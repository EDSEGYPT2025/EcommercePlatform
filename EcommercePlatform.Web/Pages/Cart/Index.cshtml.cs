// Pages/Cart/Index.cshtml.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePlatform.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly IStoreService _storeService;

        public IndexModel(ICartService cartService, IStoreService storeService)
        {
            _cartService = cartService;
            _storeService = storeService;
        }

        public ShoppingCart Cart { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StoreSlug { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = _cartService.GetCart();
            return Page();
        }

        public IActionResult OnPostUpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToPage(new { storeSlug = StoreSlug });
        }

        public IActionResult OnPostRemove(int productId)
        {
            _cartService.RemoveFromCart(productId);
            TempData["Success"] = "تم حذف المنتج من السلة";
            return RedirectToPage(new { storeSlug = StoreSlug });
        }

        public IActionResult OnPostClear()
        {
            _cartService.ClearCart();
            TempData["Success"] = "تم تفريغ السلة";
            return RedirectToPage(new { storeSlug = StoreSlug });
        }
    }
}