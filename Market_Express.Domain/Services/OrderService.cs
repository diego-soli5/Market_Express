using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Order;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.Order;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderOptions _orderOptions;
        private readonly PaginationOptions _paginationOptions;

        public OrderService(IUnitOfWork unitOfWork,
                            IOptions<OrderOptions> orderOptions,
                            IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _orderOptions = orderOptions.Value;
            _paginationOptions = paginationOptions.Value;
        }

        public PagedList<Order> GetAllByUserId(Guid userId, ClientOrderQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            var lstOrders = _unitOfWork.Order.GetAllByUserId(userId);

            if (filters.StartDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate >= filters.StartDate);

            if (filters.EndDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate <= filters.StartDate);

            if (filters.Status != null)
                lstOrders = lstOrders.Where(o => o.Status == filters.Status);

            var pagedOrders = PagedList<Order>.Create(lstOrders, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedOrders;
        }

        public async Task<List<RecentOrder>> GetRecentOrdersByUserId(Guid userId)
        {
            return await _unitOfWork.Order.GetMostRecentByUserId(userId, _orderOptions.DefaultTakeForMostRecentByUser);
        }

        public async Task<OrderStats> GetOrderStatsByUserId(Guid userId)
        {
            return await _unitOfWork.Order.GetStatsByUserId(userId);
        }

        public async Task<BusisnessResult> Generate(Guid userId)
        {
            BusisnessResult oResult = new();
            Order oOrder;
            List<OrderDetail> lstOrderDetail;

            decimal dcTotal = 0;

            var oCart = await _unitOfWork.Cart.GetCurrentByUserId(userId);

            if (oCart == null)
            {
                oResult.Message = "Se deben agregar artículos al carrito.";

                return oResult;
            }

            if (oCart.Status == CartStatus.CERRADO)
            {
                oResult.Message = "Ya se ha generado un pedido con el carrito.";

                return oResult;
            }

            var lstCartDetailsFromDb = _unitOfWork.CartDetail.GetAllByCartId(oCart.Id);

            if (lstCartDetailsFromDb?.Count() <= 0)
            {
                oResult.Message = "Se deben agregar artículos al carrito.";

                return oResult;
            }

            oOrder = new()
            {
                Id = new Guid(),
                ClientId = oCart.ClientId,
                CreationDate = DateTimeUtility.NowCostaRica,
                Status = OrderStatus.PENDIENTE
            };

            lstOrderDetail = new();

            foreach (var cartDetail in lstCartDetailsFromDb.ToList())
            {
                var oArticleAux = await _unitOfWork.Article.GetByIdAsync(cartDetail.ArticleId);

                dcTotal += oArticleAux.Price * cartDetail.Quantity;

                lstOrderDetail.Add(new OrderDetail
                {
                    ArticleId = cartDetail.ArticleId,
                    BarCode = oArticleAux.BarCode,
                    Description = oArticleAux.Description,
                    Price = oArticleAux.Price,
                    Quantity = cartDetail.Quantity,
                    Id = new Guid(),
                    OrderId = oOrder.Id
                });
            }

            oOrder.Total = dcTotal;
            oOrder.OrderDetails = lstOrderDetail;

            oCart.Status = CartStatus.CERRADO;

            _unitOfWork.Order.Create(oOrder);
            _unitOfWork.Cart.Update(oCart);

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

            oResult.Message = "El pedido se generó correctamente!";

            oResult.Data = oOrder.Id;

            return oResult;
        }
    }
}
