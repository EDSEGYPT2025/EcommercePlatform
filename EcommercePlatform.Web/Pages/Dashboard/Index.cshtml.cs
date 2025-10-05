// Pages/Dashboard/Index.cshtml.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;

namespace EcommercePlatform.Web.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IStoreService storeService, UserManager<ApplicationUser> userManager)
        {
            _storeService = storeService;
            _userManager = userManager;
        }

        public List<Store> Stores { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Stores = await _storeService.GetUserStoresAsync(user.Id);
            return Page();
        }
    }
}
