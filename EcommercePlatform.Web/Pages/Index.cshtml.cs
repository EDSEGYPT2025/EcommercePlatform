// Pages/Index.cshtml.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Store> FeaturedStores { get; set; }
        public int TotalStores { get; set; }
        public int TotalProducts { get; set; }

        public async Task OnGetAsync()
        {
            FeaturedStores = await _context.Stores
                .Include(s => s.Settings)
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Take(6)
                .ToListAsync();

            TotalStores = await _context.Stores.CountAsync(s => s.IsActive);
            TotalProducts = await _context.Products.CountAsync(p => p.IsActive);
        }
    }
}