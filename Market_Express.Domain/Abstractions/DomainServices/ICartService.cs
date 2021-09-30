using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface ICartService
    {
        Task<int> GetArticlesCount(Guid userId);
    }
}
