using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace EcommercePlatform.Web.Pages.Stores
{
    public class ViewModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ViewModel(
            IStoreService storeService,
            IProductService productService,
            ICartService cartService,
            UserManager<ApplicationUser> userManager)
        {
            _storeService = storeService;
            _productService = productService;
            _cartService = cartService;
            _userManager = userManager;
        }

        public Store Store { get; set; }
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(string storeSlug)
        {
            Store = await _storeService.GetStoreBySlugAsync(storeSlug);

            if (Store == null || !Store.IsActive)
            {
                return NotFound();
            }

            Products = await _productService.GetStoreProductsAsync(Store.Id);
            Products = Products.Where(p => p.IsActive).ToList();

            return Page();
        }

        [Authorize] // يتطلب تسجيل الدخول لإضافة المنتج للسلة
        public async Task<IActionResult> OnPostAddToCartAsync(int productId, string storeSlug)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // إذا لم يكن المستخدم مسجلاً، اطلب منه تسجيل الدخول
                return Challenge();
            }

            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null || !product.IsActive || product.Stock <= 0)
            {
                TempData["Error"] = "المنتج غير متوفر حالياً.";
                return RedirectToPage(new { storeSlug });
            }

            // --- تم التصحيح لاستدعاء الدالة الصحيحة بالمعلومات الصحيحة ---
            await _cartService.AddToCartAsync(user.Id, productId, 1);

            TempData["Success"] = $"تمت إضافة '{product.Name}' إلى السلة بنجاح!";
            return RedirectToPage(new { storeSlug });
        }
    }
}

