using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommercePlatform.Web.Pages.Dashboard.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IStoreService _storeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IProductService productService, IStoreService storeService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _storeService = storeService;
            _userManager = userManager;
        }

        public IEnumerable<Product> Products { get; set; }
        public Store CurrentStore { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var store = await _storeService.GetStoreByUserIdAsync(user.Id);

            if (store == null)
            {
                // إذا لم يكن لدى المستخدم متجر، قم بإعادة توجيهه لإنشاء واحد
                return RedirectToPage("/Dashboard/Stores/Create");
            }
            CurrentStore = store;
            Products = await _productService.GetProductsByStoreIdAsync(store.Id);
            return Page();
        }
    }
}
