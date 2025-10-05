// Pages/Account/Register.cshtml.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using EcommercePlatform.Core.Entities;

namespace EcommercePlatform.Web.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "الاسم الكامل مطلوب")]
            [Display(Name = "الاسم الكامل")]
            [StringLength(100, ErrorMessage = "الاسم يجب أن يكون بين {2} و {1} حرف", MinimumLength = 3)]
            public string FullName { get; set; }

            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
            [Display(Name = "البريد الإلكتروني")]
            public string Email { get; set; }

            [Required(ErrorMessage = "كلمة المرور مطلوبة")]
            [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون بين {2} و {1} حرف", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName = Input.FullName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // إضافة دور StoreOwner للمستخدم الجديد
                    await _userManager.AddToRoleAsync(user, "StoreOwner");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    TempData["Success"] = "تم إنشاء حسابك بنجاح!";
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, TranslateError(error.Description));
                }
            }

            return Page();
        }

        private string TranslateError(string error)
        {
            // ترجمة رسائل الخطأ الشائعة
            if (error.Contains("is already taken"))
                return "البريد الإلكتروني مستخدم بالفعل";
            if (error.Contains("Passwords must have at least one uppercase"))
                return "كلمة المرور يجب أن تحتوي على حرف كبير واحد على الأقل";
            if (error.Contains("Passwords must have at least one lowercase"))
                return "كلمة المرور يجب أن تحتوي على حرف صغير واحد على الأقل";
            if (error.Contains("Passwords must have at least one digit"))
                return "كلمة المرور يجب أن تحتوي على رقم واحد على الأقل";

            return error;
        }
    }
}