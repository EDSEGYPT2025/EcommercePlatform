using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}

