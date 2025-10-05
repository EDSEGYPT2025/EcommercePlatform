// Pages/Stores/View.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;

namespace EcommercePlatform.Web.Pages.Stores
{
    public class ViewModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;

        public ViewModel(IStoreService storeService, IProductService productService)
        {
            _storeService = storeService;
            _productService = productService;
        }

        public Store Store { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            Store = await _storeService.GetStoreBySlugAsync(slug);

            if (Store == null || !Store.IsActive)
            {
                return NotFound();
            }

            Products = await _productService.GetStoreProductsAsync(Store.Id);

            // تصفية المنتجات حسب التصنيف
            if (CategoryId.HasValue)
            {
                Products = Products.Where(p => p.CategoryId == CategoryId.Value).ToList();
            }

            // البحث في المنتجات
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Products = Products.Where(p =>
                    p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            // عرض المنتجات النشطة فقط
            Products = Products.Where(p => p.IsActive).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId, string slug)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null || !product.IsActive || product.Stock <= 0)
            {
                TempData["Error"] = "المنتج غير متوفر";
                return RedirectToPage(new { slug });
            }

            var cartService = HttpContext.RequestServices.GetRequiredService<ICartService>();
            cartService.AddToCart(product.Id, product.Name, product.MainImage, product.Price, 1);

            TempData["Success"] = "تمت إضافة المنتج إلى السلة";
            return RedirectToPage(new { slug });
        }
    }
}