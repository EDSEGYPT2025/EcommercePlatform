// Pages/Dashboard/Products/Index.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;

namespace EcommercePlatform.Web.Pages.Dashboard.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;

        public IndexModel(IProductService productService, IStoreService storeService)
        {
            _productService = productService;
            _storeService = storeService;
        }

        public List<Product> Products { get; set; }
        public Store CurrentStore { get; set; }
        public int StoreId { get; set; }

        public async Task<IActionResult> OnGetAsync(int storeId)
        {
            StoreId = storeId;
            CurrentStore = await _storeService.GetStoreByIdAsync(storeId);

            if (CurrentStore == null)
            {
                return NotFound();
            }

            Products = await _productService.GetStoreProductsAsync(storeId);
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int productId, int storeId)
        {
            await _productService.DeleteProductAsync(productId);
            TempData["Success"] = "تم حذف المنتج بنجاح";
            return RedirectToPage(new { storeId });
        }
    }
}