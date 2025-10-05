using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId)
        {
            return await _context.Products
                .Where(p => p.StoreId == storeId)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<List<Product>> GetStoreProductsAsync(int id)
        {
            return await _context.Products
               .Where(p => p.StoreId == id && p.IsActive)
               .Include(p => p.Images)
               .OrderByDescending(p => p.CreatedAt)
               .ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetActiveProductsCountAsync()
        {
            return await _context.Products.CountAsync(p => p.IsActive);
        }
    }
}
