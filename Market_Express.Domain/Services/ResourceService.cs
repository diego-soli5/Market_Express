using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IAzureBlobStorageService _storageService;

        public ResourceService(IAzureBlobStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<(Stream,string)> GetImage(string name)
        {
            return await _storageService.GetBlobAsync(name);
        }
    }
}
