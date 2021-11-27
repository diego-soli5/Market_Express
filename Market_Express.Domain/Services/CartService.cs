using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Cart;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork,
                           IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetArticlesCount(Guid userId)
        {
            return await _unitOfWork.Cart.GetArticlesCount(userId);
        }

        public async Task<BusisnessResult> GenerateCartByOrderId(Guid orderId, Guid userId)
        {
            BusisnessResult oResult = new();
            List<CartDetail> lstCartDetail = new();
            Cart oNewCart = null;

            bool bMustUseNewCart = false;
            int iSkipedArticles = 0;

            var oClient = await _unitOfWork.Client.GetByUserIdAsync(userId);

            if (oClient == null)
            {
                oResult.Message = "El cliente no existe";

                return oResult;
            }

            var oOrderFromDb = await _unitOfWork.Order.GetByIdAsync(orderId);

            if (oOrderFromDb == null)
            {
                oResult.Message = "El pedido no existe.";

                return oResult;
            }

            var lstOrderDetails = _unitOfWork.OrderDetail.GetAllByOrderId(oOrderFromDb.Id);

            var oCartFromDb = await _unitOfWork.Cart.GetCurrentByUserId(userId);

            if (oCartFromDb == null)
            {
                oNewCart = new()
                {
                    Id = new Guid(),
                    ClientId = oClient.Id,
                    OpeningDate = DateTimeUtility.NowCostaRica,
                    Status = CartStatus.ABIERTO
                };

                bMustUseNewCart = true;
            }
            
            var lstCartDetailsFromDb = _unitOfWork.CartDetail.GetAllByCartId(oCartFromDb.Id);

            if (lstCartDetailsFromDb?.Count() > 0)
            {
                oResult.Message = "No se puede generar porque ya hay artículos agregados al carrito.";

                return oResult;
            }

            foreach (var oOrderDetail in lstOrderDetails.ToList())
            {
                var articleToCheck = await _unitOfWork.Article.GetByIdAsync(oOrderDetail.ArticleId);

                if (articleToCheck != null)
                {
                    if (articleToCheck?.Status == EntityStatus.DESACTIVADO)
                    {
                        iSkipedArticles++;

                        continue;
                    }

                    lstCartDetail.Add(new CartDetail
                    {
                        Id = new Guid(),
                        ArticleId = oOrderDetail.ArticleId,
                        Quantity = oOrderDetail.Quantity,
                        CartId = bMustUseNewCart ? oNewCart.Id : oCartFromDb.Id
                    });
                }
                else
                {
                    iSkipedArticles++;
                }
            }

            if(bMustUseNewCart)
            {
                oNewCart.CartDetails = lstCartDetail;

                _unitOfWork.Cart.Create(oNewCart);
            }
            else
            {
                _unitOfWork.CartDetail.Create(lstCartDetail);
            }

            if (iSkipedArticles > 0)
                oResult.Message = $"Se generó el carrito correctamente pero se omitieron {iSkipedArticles} artículos porque han sido desactivados.";
            else
                oResult.Message = "Se generó el carrito correctamente!";

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            return oResult;
        }

        public async Task<(CartBillingDetails, List<ArticleForCartDetails>)> GetCartDetails(Guid userId)
        {
            Cart oCart = await _unitOfWork.Cart.GetCurrentByUserId(userId);

            if (oCart == null)
                oCart = new();

            var lstArticles = await _unitOfWork.Article.GetAllForCartDetails(userId);

            CartBillingDetails oCartBillingDetails = new()
            {
                Id = oCart.Id,
                ClientId = oCart.ClientId,
                OpeningDate = oCart.OpeningDate,
                Status = oCart.Status,
                Discount = 0,
                SubTotal = lstArticles.Select(a => a.Price * a.Quantity).Sum()
            };

            return (oCartBillingDetails, lstArticles);
        }

        public async Task<BusisnessResult> AddDetail(Guid articleId, Guid userId)
        {
            BusisnessResult oResult = new();
            Cart oNewCart = null;
            CartDetail oNewCartDetail = null;
            Client oClientFromDb = null;
            Cart oCartFromDb = null;
            CartDetail oCartDetailFromDb = null;
            bool bMustUseNewCart = false;

            if (!await ValidateEntities(oResult, articleId, userId))
                return oResult;

            oClientFromDb = _unitOfWork.Client.GetFirstOrDefault(c => c.AppUserId == userId);

            if (oClientFromDb == null)
            {
                oResult.Message = "El cliente no existe.";

                return oResult;
            }

            oCartFromDb = _unitOfWork.Cart.GetFirstOrDefault(cart => cart.ClientId == oClientFromDb.Id && cart.Status == CartStatus.ABIERTO);

            if (oCartFromDb == null)
            {
                bMustUseNewCart = true;

                oNewCart = new(Guid.NewGuid(), oClientFromDb.Id, DateTimeUtility.NowCostaRica, CartStatus.ABIERTO);

                _unitOfWork.Cart.Create(oNewCart);
            }

            if (bMustUseNewCart)
            {
                oNewCartDetail = new(Guid.NewGuid(), oNewCart.Id, articleId, 1);

                _unitOfWork.CartDetail.Create(oNewCartDetail);

                oResult.Data = oNewCartDetail.Quantity;

                oResult.Message = "El detalle se agregó al carrito.";
            }
            else
            {
                oCartDetailFromDb = _unitOfWork.CartDetail.GetFirstOrDefault(c => c.ArticleId == articleId && c.CartId == oCartFromDb.Id);

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

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            return oResult;
        }

        public async Task<BusisnessResult> UpdateDetail(bool increaseQuantity, Guid articleId, Guid userId)
        {
            BusisnessResult oResult = new();
            Cart oNewCart = null;
            CartDetail oNewCartDetail = null;
            Client oClientFromDb = null;
            Cart oCartFromDb = null;
            CartDetail oCartDetailFromDb = null;
            bool bMustUseNewCart = false;

            if (!await ValidateEntities(oResult, articleId, userId))
                return oResult;

            oClientFromDb = _unitOfWork.Client.GetFirstOrDefault(c => c.AppUserId == userId);

            if (oClientFromDb == null)
            {
                oResult.Message = "El cliente no existe.";

                return oResult;
            }

            oCartFromDb = _unitOfWork.Cart.GetFirstOrDefault(cart => cart.ClientId == oClientFromDb.Id && cart.Status == CartStatus.ABIERTO);

            if (oCartFromDb == null)
            {
                bMustUseNewCart = true;

                oNewCart = new(Guid.NewGuid(), oClientFromDb.Id, DateTimeUtility.NowCostaRica, CartStatus.ABIERTO);
            }

            if (bMustUseNewCart)
            {
                if (increaseQuantity)
                {
                    oNewCartDetail = new(Guid.NewGuid(), oNewCart.Id, articleId, 1);

                    _unitOfWork.Cart.Create(oNewCart);

                    _unitOfWork.CartDetail.Create(oNewCartDetail);

                    oResult.Data = oNewCartDetail.Quantity;

                    oResult.Message = "El detalle se agregó al carrito.";
                }
            }
            else
            {
                oCartDetailFromDb = _unitOfWork.CartDetail.GetFirstOrDefault(c => c.ArticleId == articleId && c.CartId == oCartFromDb.Id);

                if (oCartDetailFromDb != null)
                {
                    if (increaseQuantity)
                    {
                        oCartDetailFromDb.Quantity += 1;

                        oResult.Message = "Se aumentó la cantidad del detalle.";
                    }
                    else
                    {
                        oCartDetailFromDb.Quantity -= 1;

                        oResult.Message = "Se disminuyó la cantidad del detalle.";
                    }


                    if (oCartDetailFromDb.Quantity <= 0)
                    {
                        _unitOfWork.CartDetail.Delete(oCartDetailFromDb);

                        oResult.ResultCode = 0;

                        oResult.Message = "Se eliminó el detalle del carrito.";
                    }
                    else
                    {
                        _unitOfWork.CartDetail.Update(oCartDetailFromDb);

                        oResult.ResultCode = 1;
                    }

                    oResult.Data = oCartDetailFromDb.Quantity;
                }
                else
                {
                    if (increaseQuantity)
                    {
                        oNewCartDetail = new(Guid.NewGuid(), oCartFromDb.Id, articleId, 1);

                        _unitOfWork.CartDetail.Create(oNewCartDetail);

                        oResult.Data = oNewCartDetail.Quantity;

                        oResult.Message = "El detalle se agregó al carrito.";
                    }
                }
            }

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            return oResult;
        }

        public async Task<BusisnessResult> DeleteDetail(Guid articleId, Guid userId)
        {
            BusisnessResult oResult = new();
            Client oClientFromDb = null;
            Cart oCartFromDb = null;
            CartDetail oCartDetailFromDb;

            if (!await ValidateEntities(oResult, articleId, userId))
                return oResult;

            oClientFromDb = _unitOfWork.Client.GetFirstOrDefault(c => c.AppUserId == userId);

            if (oClientFromDb == null)
            {
                oResult.Message = "El cliente no existe.";

                return oResult;
            }

            oCartFromDb = _unitOfWork.Cart.GetFirstOrDefault(cart => cart.ClientId == oClientFromDb.Id && cart.Status == CartStatus.ABIERTO);

            if (oCartFromDb == null)
            {
                oResult.Message = "El detalle no existe.";

                return oResult;
            }

            oCartDetailFromDb = _unitOfWork.CartDetail.GetFirstOrDefault(c => c.ArticleId == articleId && c.CartId == oCartFromDb.Id);

            if (oCartDetailFromDb != null)
            {
                _unitOfWork.CartDetail.Delete(oCartDetailFromDb);

                oResult.Message = "Se eliminó el detalle del carrito.";
            }
            else
            {
                oResult.Message = "El detalle no existe.";

                return oResult;
            }

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            return oResult;
        }

        #region UTILITY METHODS
        private async Task<bool> ValidateEntities(BusisnessResult result, Guid articleId, Guid userId)
        {
            Article oArticleToValidate = await _unitOfWork.Article.GetByIdAsync(articleId);

            if (oArticleToValidate == null)
            {
                result.Message = "El artículo no existe.";

                return false;
            }

            if (oArticleToValidate.Status == EntityStatus.DESACTIVADO)
            {
                result.Message = "El artículo está desactivado.";

                return false;
            }

            AppUser oAppUserToValidate = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oAppUserToValidate == null)
            {
                result.Message = "El usuario no existe.";

                return false;
            }

            return true;
        }
        #endregion
    }
}
