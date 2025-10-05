using EcommercePlatform.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<List<Product>> GetStoreProductsAsync(int id);

        /// <summary>
        /// يحسب العدد الإجمالي للمنتجات النشطة في جميع المتاجر.
        /// </summary>
        /// <returns>العدد الإجمالي للمنتجات النشطة.</returns>
        Task<int> GetActiveProductsCountAsync();
    }
}
