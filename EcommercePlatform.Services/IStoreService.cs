using EcommercePlatform.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    /// <summary>
    /// واجهة لخدمات إدارة المتاجر.
    /// توفر وظائف لإنشاء المتاجر، وجلب بياناتها، وإحصائياتها.
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// ينشئ متجراً جديداً في قاعدة البيانات.
        /// </summary>
        /// <param name="store">كائن المتجر المراد إنشاؤه.</param>
        /// <returns>مهمة تكتمل عند إنشاء المتجر.</returns>
        Task CreateStoreAsync(Store store);

        /// <summary>
        /// يجلب متجراً محدداً باستخدام معرفه الفريد (Slug).
        /// </summary>
        /// <param name="slug">المعرف النصي الفريد للمتجر.</param>
        /// <returns>كائن المتجر المطابق أو null إذا لم يتم العثور عليه.</returns>
        Task<Store> GetStoreBySlugAsync(string slug);

        /// <summary>
        /// يجلب المتجر المرتبط بمعرف مستخدم معين (صاحب المتجر).
        /// </summary>
        /// <param name="userId">معرف المستخدم.</param>
        /// <returns>كائن المتجر أو null إذا لم يكن المستخدم يملك متجراً.</returns>
        Task<Store> GetStoreByUserIdAsync(string userId);

        /// <summary>
        /// يجلب قائمة بالمتاجر المميزة لعرضها في الصفحة الرئيسية.
        /// </summary>
        /// <param name="count">عدد المتاجر المراد جلبها.</param>
        /// <returns>قائمة من المتاجر المميزة.</returns>
        Task<List<Store>> GetFeaturedStoresAsync(int count);

        /// <summary>
        /// يحسب العدد الإجمالي للمتاجر النشطة.
        /// </summary>
        /// <returns>العدد الإجمالي للمتاجر النشطة.</returns>
        Task<int> GetActiveStoresCountAsync();
    }
}
