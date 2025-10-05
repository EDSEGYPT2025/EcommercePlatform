using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services; // استخدام الخدمات
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Web.Pages
{
    public class IndexModel : PageModel
    {
        // تم استبدال DbContext بالخدمات المتخصصة
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;

        public IndexModel(IStoreService storeService, IProductService productService)
        {
            _storeService = storeService;
            _productService = productService;
        }

        public List<Store> FeaturedStores { get; set; }
        public int TotalStores { get; set; }
        public int TotalProducts { get; set; }

        public async Task OnGetAsync()
        {
            // استخدام الخدمات لجلب البيانات، مما يفصل الواجهة عن قاعدة البيانات
            FeaturedStores = await _storeService.GetFeaturedStoresAsync(6);
            TotalStores = await _storeService.GetActiveStoresCountAsync();
            TotalProducts = await _productService.GetActiveProductsCountAsync();
        }
    }
}
