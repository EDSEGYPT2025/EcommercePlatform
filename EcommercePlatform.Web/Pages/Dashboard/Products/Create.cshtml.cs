// تحديث صفحة إضافة المنتج لتشمل رفع الصور
// Pages/Dashboard/Products/Create.cshtml.cs - تحديث
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Web.Pages.Dashboard.Products
{
    [Authorize]
    public class CreateModelUpdated : PageModel
    {
        private readonly IProductService _productService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ApplicationDbContext _context;

        public CreateModelUpdated(IProductService productService, IFileUploadService fileUploadService, ApplicationDbContext context)
        {
            _productService = productService;
            _fileUploadService = fileUploadService;
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public int StoreId { get; set; }

        [BindProperty]
        public IFormFile MainImageFile { get; set; }

        [BindProperty]
        public List<IFormFile> AdditionalImages { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int storeId)
        {
            StoreId = storeId;
            await LoadCategories(storeId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(StoreId);
                return Page();
            }

            try
            {
                // رفع الصورة الرئيسية
                if (MainImageFile != null)
                {
                    Product.MainImage = await _fileUploadService.UploadImageAsync(MainImageFile, "products");
                }

                Product.StoreId = StoreId;
                var createdProduct = await _productService.CreateProductAsync(Product);

                // رفع الصور الإضافية
                if (AdditionalImages != null && AdditionalImages.Any())
                {
                    int displayOrder = 1;
                    foreach (var imageFile in AdditionalImages)
                    {
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            var imagePath = await _fileUploadService.UploadImageAsync(imageFile, "products");

                            var productImage = new ProductImage
                            {
                                ProductId = createdProduct.Id,
                                ImageUrl = imagePath,
                                DisplayOrder = displayOrder++
                            };

                            _context.ProductImages.Add(productImage);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "تم إضافة المنتج بنجاح!";
                return RedirectToPage("/Dashboard/Products/Index", new { storeId = StoreId });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                await LoadCategories(StoreId);
                return Page();
            }
        }

        private async Task LoadCategories(int storeId)
        {
            var categories = await _context.Categories
                .Where(c => c.StoreId == storeId)
                .ToListAsync();

            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }
    }
}