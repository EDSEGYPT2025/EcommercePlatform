using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommercePlatform.Services;
public interface IFileUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string folder = "products");
    Task<bool> DeleteImageAsync(string imagePath);
    bool IsValidImage(IFormFile file);
}
