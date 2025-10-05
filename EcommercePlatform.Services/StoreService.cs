using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// التنفيذ الفعلي لخدمات إدارة المتاجر.
    /// </summary>
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task CreateStoreAsync(Store store)
        {
            // عند إنشاء متجر جديد، قم بإنشاء إعدادات افتراضية له تلقائياً
            store.Settings = new StoreSettings();
            await _context.Stores.AddAsync(store);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            return await _context.Stores.FindAsync(storeId);
        }

        /// <inheritdoc />
        public async Task<Store> GetStoreBySlugAsync(string slug)
        {
            // استخدام Include لجلب البيانات المرتبطة (صاحب المتجر والإعدادات) في استعلام واحد
            return await _context.Stores
                .Include(s => s.Owner)
                .Include(s => s.Settings)
                .FirstOrDefaultAsync(s => s.Slug == slug);
        }

        /// <inheritdoc />
        public async Task<Store> GetStoreByUserIdAsync(string userId)
        {
            return await _context.Stores
                .Include(s => s.Settings)
                .FirstOrDefaultAsync(s => s.OwnerId == userId);
        }

        /// <inheritdoc />
        public async Task<List<Store>> GetFeaturedStoresAsync(int count)
        {
            return await _context.Stores
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetActiveStoresCountAsync()
        {
            return await _context.Stores.CountAsync(s => s.IsActive);
        }

        /// <inheritdoc />
        public async Task<bool> IsSlugAvailableAsync(string slug)
        {
            return !await _context.Stores.AnyAsync(s => s.Slug == slug);
        }
    }
}

