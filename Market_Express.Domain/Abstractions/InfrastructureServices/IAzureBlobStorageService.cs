using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.InfrastructureServices
{
    public interface IAzureBlobStorageService
    {
        Task<string> CreateBlobAsync(IFormFile file);
        Task<bool> DeleteBlobAsync(string fileName);
        Task<(Stream, string)> GetBlobAsync(string fileName);
    }
}