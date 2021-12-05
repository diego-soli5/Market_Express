using System.IO;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IResourceService
    {
        Task<(Stream, string)> GetImage(string name);
    }
}
