using CRM.Application.Interfaces;
using CRM.Infrastructure.FileStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CRM.Application.Services
{
    internal class FileStoreLocalApplication : IFileStoreLocalApplication
    {
        private readonly IWebHostEnvironment _env; //Ruta Raiz del Api
        private readonly IHttpContextAccessor _contextAccessor; //ACCESO AL CONTESTO DE LA SOCITUTD
        private readonly IFileStorageLocal _fileStorageLocal;

        public FileStoreLocalApplication(IFileStorageLocal fileStorageLocal, IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
        {
            _fileStorageLocal = fileStorageLocal;
            _env = env;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            //var webRootPath = @"C:\POSSOARSA";
            var webRootPath = _env.WebRootPath;
            var schema = _contextAccessor.HttpContext!.Request.Scheme;
            var host = _contextAccessor.HttpContext.Request.Host;

            return await _fileStorageLocal.SaveFile(container, file, webRootPath, schema, host.Value);
        }

        public async Task<string> EditFile(string container, IFormFile file, string route)
        {
            var webRootPath = _env.WebRootPath;
            //var webRootPath = @"C:\POSSOARSA";
            var schema = _contextAccessor.HttpContext!.Request.Scheme;
            var host = _contextAccessor.HttpContext.Request.Host;

            return await _fileStorageLocal.EditFile(container, file, route, webRootPath, schema, host.Value);
        }

        public async Task RemoveFile(string route, string container)
        {
            var webRootPath = _env.WebRootPath;
            await _fileStorageLocal.RemoveFile(route, container, webRootPath);

        }
    }
}
