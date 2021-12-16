using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.ExternalServices.Azure
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private readonly BlobServiceClient _blobService;
        private readonly AzureBlobStorageOptions _options;
        private readonly BlobContainerClient _container;

        public AzureBlobStorageService(BlobServiceClient blobService, IOptions<AzureBlobStorageOptions> options)
        {
            _blobService = blobService;
            _options = options.Value;
            _container = _blobService.GetBlobContainerClient(_options.ContainerName);
        }

        public async Task<(Stream, string)> GetBlobAsync(string fileName)
        {
            var blobClient = _container.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var blobDownloadInfo = await blobClient.DownloadAsync();

                return (blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
            }

            return (null, null);
        }

        public async Task<string> CreateBlobAsync(IFormFile file)
        {
            string newFileName = $"Img_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var blobClient = _container.GetBlobClient(newFileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });
            }

            return newFileName;
        }

        public async Task<bool> DeleteBlobAsync(string fileName)
        {
            var blobClient = _container.GetBlobClient(fileName ?? "");

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
