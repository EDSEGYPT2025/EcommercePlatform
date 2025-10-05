using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EcommercePlatform.Web.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStoreService _storeService;

        public IndexModel(UserManager<ApplicationUser> userManager, IStoreService storeService)
        {
            _userManager = userManager;
            _storeService = storeService;
        }

        public Store UserStore { get; set; }

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            // تم تصحيح اسم الدالة هنا
            UserStore = await _storeService.GetUserStoresAsync(userId);
        }
    }
}
