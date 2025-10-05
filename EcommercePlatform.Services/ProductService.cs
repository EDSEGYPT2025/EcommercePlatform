// ProductService.cs
using EcommercePlatform.Core.Entities;
using EcommercePlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Services
{
    // ProductService.cs
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetStoreProductsAsync(int storeId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.StoreId == storeId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.Slug = GenerateSlug(product.Name);
            product.CreatedAt = DateTime.UtcNow;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.Stock -= quantity;
            return await _context.SaveChangesAsync() > 0;
        }

        private string GenerateSlug(string name)
        {
            return name.ToLower().Replace(" ", "-");
        }
    }
}
