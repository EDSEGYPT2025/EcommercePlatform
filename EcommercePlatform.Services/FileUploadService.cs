using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
       
        private readonly IConfiguration _configuration;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB

        public FileUploadService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folder = "products")
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("ملف غير صالح");
            }

            if (!IsValidImage(file))
            {
                throw new ArgumentException("نوع الملف غير مدعوم. الرجاء رفع صورة بصيغة JPG, PNG, GIF, أو WEBP");
            }

            // إنشاء اسم فريد للملف
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // المسار الكامل
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);

            // إنشاء المجلد إذا لم يكن موجوداً
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            // حفظ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // إرجاع المسار النسبي
            return $"/uploads/{folder}/{fileName}";
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            // التحقق من الحجم
            if (file.Length > _maxFileSize)
                return false;

            // التحقق من الامتداد
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
                return false;

            // التحقق من نوع المحتوى
            var validContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
            if (!validContentTypes.Contains(file.ContentType.ToLower()))
                return false;

            return true;
        }
    }
}
