using EcommercePlatform.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// واجهة لخدمات إدارة المنتجات.
    /// توفر وظائف لإنشاء وتعديل وحذف وجلب بيانات المنتجات.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// يجلب منتجاً محدداً باستخدام معرفه الرقمي.
        /// </summary>
        /// <param name="productId">المعرف الرقمي للمنتج.</param>
        /// <returns>كائن المنتج المطابق أو null إذا لم يتم العثور عليه.</returns>
        Task<Product> GetProductByIdAsync(int productId);

        /// <summary>
        /// يجلب كل المنتجات التابعة لمتجر معين.
        /// </summary>
        /// <param name="storeId">المعرف الرقمي للمتجر.</param>
        /// <returns>قائمة بمنتجات المتجر.</returns>
        Task<IEnumerable<Product>> GetProductsByStoreIdAsync(int storeId);

        /// <summary>
        /// ينشئ منتجاً جديداً في قاعدة البيانات.
        /// </summary>
        /// <param name="product">كائن المنتج المراد إنشاؤه.</param>
        Task CreateProductAsync(Product product);

        /// <summary>
        /// يحدّث بيانات منتج موجود.
        /// </summary>
        /// <param name="product">كائن المنتج مع البيانات المحدثة.</param>
        Task UpdateProductAsync(Product product);

        /// <summary>
        /// يحذف منتجاً من قاعدة البيانات.
        /// </summary>
        /// <param name="productId">المعرف الرقمي للمنتج المراد حذفه.</param>
        Task DeleteProductAsync(int productId);

        /// <summary>
        /// يجلب قائمة بالمنتجات النشطة فقط لمتجر معين.
        /// </summary>
        /// <param name="id">المعرف الرقمي للمتجر.</param>
        /// <returns>قائمة بالمنتجات النشطة.</returns>
        Task<List<Product>> GetStoreProductsAsync(int id);

        /// <summary>
        /// يحسب العدد الإجمالي للمنتجات النشطة في جميع المتاجر.
        /// </summary>
        /// <returns>العدد الإجمالي للمنتجات النشطة.</returns>
        Task<int> GetActiveProductsCountAsync();
    }
}

