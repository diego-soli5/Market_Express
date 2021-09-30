using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetArticlesCount(Guid userId)
        {
            return await _unitOfWork.Cart.GetArticlesCount(userId);
        }
    }
}
