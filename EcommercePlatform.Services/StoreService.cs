// StoreService.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Services
{
    // StoreService.cs
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores
                .Include(s => s.Settings)
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.Id == storeId);
        }

        public async Task<Store> GetStoreBySlugAsync(string slug)
        {
            return await _context.Stores
                .Include(s => s.Settings)
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.Slug == slug && s.IsActive);
        }

        public async Task<List<Store>> GetUserStoresAsync(string userId)
        {
            return await _context.Stores
                .Include(s => s.SubscriptionPlan)
                .Where(s => s.OwnerId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Store> CreateStoreAsync(Store store)
        {
            store.Slug = GenerateSlug(store.Name);
            store.CreatedAt = DateTime.UtcNow;

            _context.Stores.Add(store);

            // إنشاء إعدادات افتراضية للمتجر
            var settings = new StoreSettings
            {
                Store = store,
                Currency = "EGP",
                Language = "ar",
                ShippingFee = 0,
                FreeShippingEnabled = false
            };

            _context.StoreSettings.Add(settings);
            await _context.SaveChangesAsync();

            return store;
        }

        public async Task<bool> UpdateStoreAsync(Store store)
        {
            _context.Stores.Update(store);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteStoreAsync(int storeId)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null) return false;

            _context.Stores.Remove(store);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsSlugAvailableAsync(string slug, int? excludeStoreId = null)
        {
            var query = _context.Stores.Where(s => s.Slug == slug);

            if (excludeStoreId.HasValue)
            {
                query = query.Where(s => s.Id != excludeStoreId.Value);
            }

            return !await query.AnyAsync();
        }

        private string GenerateSlug(string name)
        {
            var slug = name.ToLower()
                .Replace(" ", "-")
                .Replace("أ", "ا")
                .Replace("إ", "ا")
                .Replace("آ", "ا");

            return slug;
        }
    }
}
