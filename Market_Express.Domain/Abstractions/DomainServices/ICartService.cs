using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Cart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface ICartService
    {
        Task<int> GetArticlesCount(Guid userId);
        Task<BusisnessResult> AddDetail(Guid articleId, Guid userId);
        Task<BusisnessResult> UpdateDetail(bool plus, Guid articleId, Guid userId);
        Task<BusisnessResult> DeleteDetail(Guid articleId, Guid userId);
        Task<(CartBillingDetails, List<ArticleForCartDetails>)> GetCartDetails(Guid userId);
        Task<BusisnessResult> GenerateCartByOrderId(Guid orderId, Guid userId);
    }
}
