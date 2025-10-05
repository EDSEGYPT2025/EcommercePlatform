using EcommercePlatform.Core.Entities;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;

namespace EcommercePlatform.Web.Pages.Dashboard.Products
{
    [Authorize] // الخطوة 3: إضافة صلاحيات الأمان
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IStoreService _storeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(
            IProductService productService,
            IFileUploadService fileUploadService,
            IStoreService storeService,
            UserManager<ApplicationUser> userManager,
            ILogger<CreateModel> logger)
        {
            _productService = productService;
            _fileUploadService = fileUploadService;
            _storeService = storeService;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public ProductInputModel ProductInput { get; set; }

        public class ProductInputModel
        {
            [Required]
            public string Name { get; set; }
            public string Description { get; set; }
            [Required]
            [Range(0.01, 1000000)]
            public decimal Price { get; set; }
            [Required]
            [Range(0, 100000)]
            public int Stock { get; set; }
            [Required]
            public int CategoryId { get; set; } = 1; // Default to a category
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // الخطوة 1: إصلاح StoreId الثابت
                var userId = _userManager.GetUserId(User);
                var store = await _storeService.GetStoreByUserIdAsync(userId);

                if (store == null)
                {
                    ModelState.AddModelError(string.Empty, "You must create a store before adding products.");
                    return Page();
                }

                var product = new Product
                {
                    Name = ProductInput.Name,
                    Description = ProductInput.Description,
                    Price = ProductInput.Price,
                    Stock = ProductInput.Stock,
                    CategoryId = ProductInput.CategoryId,
                    StoreId = store.Id // استخدام المعرف الصحيح للمتجر
                };

                // Handle file upload if a file is provided
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    if (file != null && file.Length > 0)
                    {
                        var imageUrl = await _fileUploadService.UploadFileAsync(file);
                        product.Images = new List<ProductImage> { new ProductImage { Url = imageUrl } };
                    }
                }

                await _productService.CreateProductAsync(product);

                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // الخطوة 5: تطبيق معالجة قوية للأخطاء
                _logger.LogError(ex, "An error occurred while creating a product.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return Page();
            }
        }
    }
}
