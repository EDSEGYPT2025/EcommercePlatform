using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public class StoreService : IStoreService
    {
        private readonly ApplicationDbContext _context;

        public StoreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateStoreAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        public Task<bool> DeleteStoreAsync(int storeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Store>> GetAllStoresAsync()
        {
            return await _context.Stores.ToListAsync();
        }
        public async Task<Store> GetStoreByIdAsync(int id)
        {
            return await _context.Stores.FindAsync(id);
        }

        public Task<Store> GetStoreBySlugAsync(string slug)
        {
            throw new NotImplementedException();
        }

        public async Task<Store> GetStoreByUserIdAsync(string userId)
        {
            return await _context.Stores.FirstOrDefaultAsync(s => s.OwnerId == userId);
        }

        public Task<List<Store>> GetUserStoresAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSlugAvailableAsync(string slug, int? excludeStoreId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStoreAsync(Store store)
        {
            throw new NotImplementedException();
        }

        Task<Store> IStoreService.CreateStoreAsync(Store store)
        {
            throw new NotImplementedException();
        }
    }
}
