using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IAzureBlobStorageService _storageService;
        private readonly IHostEnvironment _hostEnvironment;

        public ResourceService(IAzureBlobStorageService storageService,
                               IHostEnvironment hostEnvironment)
        {
            _storageService = storageService;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<(Stream, string)> GetImage(string name)
        {
            return await _storageService.GetBlobAsync(name);
        }

        public Stream GetUserManual(bool isAdmin)
        {
            string rootPath = _hostEnvironment.ContentRootPath;
            string fileName = isAdmin ? "MANUAL_DE USUARIO_ADMINISTRADOR.pdf"
                                      : "MANUAL_DE_USUARIO_CLIENTE.pdf";
            string filePath = $"{rootPath}/wwwroot/{fileName}";

            var fs = new FileStream(filePath, FileMode.Open);

            return fs;
        }
    }
}
