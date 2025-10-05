using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace EcommercePlatform.Web.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public ShoppingCart Cart { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Cart = await _cartService.GetCartAsync(user.Id);
            }
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int cartItemId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // --- تم التصحيح هنا لإرسال معرف المستخدم ومعرف عنصر السلة ---
                await _cartService.RemoveItemFromCartAsync(user.Id, cartItemId);
                TempData["Success"] = "تم حذف المنتج من السلة بنجاح.";
            }

            return RedirectToPage();
        }
    }
}

