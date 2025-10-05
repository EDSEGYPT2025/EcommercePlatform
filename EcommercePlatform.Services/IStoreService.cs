// IStoreService.cs
using EcommercePlatform.Core.Entities;

namespace EcommercePlatform.Services
{
    public interface IStoreService
    {
        Task<Store> GetStoreByIdAsync(int storeId);
        Task<Store> GetStoreBySlugAsync(string slug);
        Task<List<Store>> GetUserStoresAsync(string userId);
        Task<Store> CreateStoreAsync(Store store);
        Task<bool> UpdateStoreAsync(Store store);
        Task<bool> DeleteStoreAsync(int storeId);
        Task<bool> IsSlugAvailableAsync(string slug, int? excludeStoreId = null);

        Task<IEnumerable<Store>> GetAllStoresAsync();
        Task<Store> GetStoreByUserIdAsync(string userId);
    }
}