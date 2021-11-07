using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
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

        public async Task<BusisnessResult> AddDetail(Guid articleId, Guid userId)
        {
            BusisnessResult oResult = new();
            Cart oNewCart = null;
            CartDetail oNewCartDetail = null;

            bool bMustUseNewCart = false;

            Article oArticleFromDb = await _unitOfWork.Article.GetByIdAsync(articleId);

            if (oArticleFromDb == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            AppUser oUserFromDb = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUserFromDb == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            Client oClient = _unitOfWork.Client.GetFirstOrDefault(c => c.AppUserId == userId);

            if (oClient == null)
            {
                oResult.Message = "El cliente no existe.";

                return oResult;
            }

            Cart oCartFromDb = _unitOfWork.Cart.GetFirstOrDefault(cart => cart.ClientId == oClient.Id && cart.Status == CartStatus.ABIERTO);

            if (oCartFromDb == null)
            {
                bMustUseNewCart = true;

                oNewCart = new(Guid.NewGuid(), oClient.Id, DateTimeUtility.NowCostaRica, CartStatus.ABIERTO);

                _unitOfWork.Cart.Create(oNewCart);
            }

            if (!bMustUseNewCart)
            {
                CartDetail oCartDetailFromDb = _unitOfWork.CartDetail.GetFirstOrDefault(c => c.ArticleId == articleId);

                if (oCartDetailFromDb == null)
                {
                    oNewCartDetail = new(Guid.NewGuid(), oCartFromDb.Id, articleId, 1);

                    _unitOfWork.CartDetail.Create(oNewCartDetail);

                    oResult.Data = oNewCartDetail.Quantity;

                    oResult.Message = "El detalle se agregó al carrito.";
                }
                else
                {
                    oCartDetailFromDb.Quantity += 1;

                    _unitOfWork.CartDetail.Update(oCartDetailFromDb);

                    oResult.Data = oCartDetailFromDb.Quantity;

                    oResult.Message = "Se aumentó la cantidad del artículo en el carrito.";
                }
            }
            else
            {
                oNewCartDetail = new(Guid.NewGuid(), oNewCart.Id, articleId, 1);

                _unitOfWork.CartDetail.Create(oNewCartDetail);

                oResult.Data = oNewCartDetail.Quantity;

                oResult.Message = "El detalle se agregó al carrito.";
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                oResult.Success = await _unitOfWork.Save();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();

                throw ex;
            }

            return oResult;
        }

        public async Task<BusisnessResult> UpdateDetail(bool plus, Guid articleId, Guid userId)
        {
            BusisnessResult oResult = new();

            var oArticleFromDb = await _unitOfWork.Article.GetByIdAsync(articleId);

            if (oArticleFromDb == null)
            {
                oResult.Message = "El artículo no existe.";

                return oResult;
            }

            var oUserFromDb = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUserFromDb == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            return oResult;
        }
    }
}
