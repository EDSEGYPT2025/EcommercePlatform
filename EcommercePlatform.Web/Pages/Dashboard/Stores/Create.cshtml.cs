// Pages/Dashboard/Stores/Create.cshtml.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Web.Pages.Dashboard.Stores
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IStoreService _storeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CreateModel(IStoreService storeService, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _storeService = storeService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Store Store { get; set; }

        public List<SelectListItem> SubscriptionPlans { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadSubscriptionPlans();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadSubscriptionPlans();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Store.OwnerId = user.Id;

            // التحقق من توفر الاسم المستعار
            if (!await _storeService.IsSlugAvailableAsync(Store.Slug))
            {
                ModelState.AddModelError("Store.Slug", "هذا الاسم المستعار محجوز بالفعل");
                await LoadSubscriptionPlans();
                return Page();
            }

            var createdStore = await _storeService.CreateStoreAsync(Store);

            TempData["Success"] = "تم إنشاء المتجر بنجاح!";
            return RedirectToPage("/Dashboard/Stores/Manage", new { id = createdStore.Id });
        }

        private async Task LoadSubscriptionPlans()
        {
            var plans = await _context.SubscriptionPlans.ToListAsync();
            SubscriptionPlans = plans.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} - {p.MonthlyPrice} جنيه/شهر"
            }).ToList();
        }
    }
}