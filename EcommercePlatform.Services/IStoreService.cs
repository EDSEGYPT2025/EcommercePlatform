using EcommercePlatform.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public interface IStoreService
    {
        Task CreateStoreAsync(Store store);
        Task<Store> GetStoreBySlugAsync(string slug);
        Task<Store> GetStoreByUserIdAsync(string userId);
        Task<List<Store>> GetFeaturedStoresAsync(int count);
        Task<int> GetActiveStoresCountAsync();

        // --- الإضافات الجديدة لحل الأخطاء ---
        Task<Store> GetStoreByIdAsync(int storeId);
        Task<bool> IsSlugAvailableAsync(string slug);
    }
}

