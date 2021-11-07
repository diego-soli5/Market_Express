using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface ICartService
    {
        Task<int> GetArticlesCount(Guid userId);
        Task<BusisnessResult> UpdateDetail(bool plus, Guid articleId, Guid userId);
        Task<BusisnessResult> AddDetail(Guid articleId, Guid userId);
    }
}
