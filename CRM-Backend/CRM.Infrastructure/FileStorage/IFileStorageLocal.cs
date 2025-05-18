using Microsoft.AspNetCore.Http;

namespace CRM.Infrastructure.FileStorage

{
    public interface IFileStorageLocal
    {
        Task<string> SaveFile(string container, IFormFile file, string webRootPath, string schema, string host);
        Task<string> EditFile(string container, IFormFile file, string route, string webRootPath, string schema, string host);
        Task RemoveFile(string root, string container, string webRootPath);
    }
}
