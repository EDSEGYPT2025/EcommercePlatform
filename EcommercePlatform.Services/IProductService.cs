// IProductService.cs
using EcommercePlatform.Core.Entities;

namespace EcommercePlatform.Services
{
    // IProductService.cs
    public interface IProductService
    {
        Task<List<Product>> GetStoreProductsAsync(int storeId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> UpdateStockAsync(int productId, int quantity);
    }
}
