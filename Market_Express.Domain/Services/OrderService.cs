using Market_Express.CrossCutting.CustomExceptions;
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
    public class OrderService : BaseService, IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderOptions _orderOptions;
        private readonly PaginationOptions _paginationOptions;

        public OrderService(IUnitOfWork unitOfWork,
                            IOptions<OrderOptions> orderOptions,
                            IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _orderOptions = orderOptions.Value;
            _paginationOptions = paginationOptions.Value;
        }

        public PagedList<Order> GetAllPaginated(AdminOrderQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            var lstOrders = _unitOfWork.Order.GetAllIncludeAppUser();

            if (filters.StartDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate.Date >= filters.StartDate.Value.Date);

            if (filters.EndDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate.Date <= filters.EndDate.Value.Date);

            if (filters.Status != null)
                lstOrders = lstOrders.Where(o => o.Status == filters.Status);

            if (filters.ClientName != null)
                lstOrders = lstOrders.Where(o => o.Client.AppUser.Name.Trim().ToUpper().Contains(filters.ClientName.Trim().ToUpper()));

            lstOrders = lstOrders.OrderByDescending(o => o.CreationDate);

            var pagedOrders = PagedList<Order>.Create(lstOrders, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedOrders;
        }

        public PagedList<Order> GetAllPaginatedByUserId(Guid userId, MyOrdersQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            var lstOrders = _unitOfWork.Order.GetAllByUserId(userId);

            if (filters.StartDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate.Date >= filters.StartDate.Value.Date);

            if (filters.EndDate != null)
                lstOrders = lstOrders.Where(o => o.CreationDate.Date <= filters.EndDate.Value.Date);

            if (filters.Status != null)
                lstOrders = lstOrders.Where(o => o.Status == filters.Status);

            lstOrders = lstOrders.OrderByDescending(o => o.CreationDate);

            var pagedOrders = PagedList<Order>.Create(lstOrders, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedOrders;
        }

        public async Task<List<RecentOrder>> GetMostRecentByUserId(Guid userId)
        {
            return await _unitOfWork.Order.GetMostRecentByUserId(userId, _orderOptions.DefaultTakeForMostRecentByUser);
        }

        public async Task<List<RecentOrder>> GetMostRecent()
        {
            return await _unitOfWork.Order.GetMostRecent(_orderOptions.DefaultTakeForMostRecent, true);
        }

        public async Task<OrderStats> GetOrderStatsByUserId(Guid userId)
        {
            return await _unitOfWork.Order.GetStatsByUserId(userId);
        }

        public async Task<OrderStats> GetOrderStats()
        {
            return await _unitOfWork.Order.GetStats();
        }

        public async Task<Order> GetById(Guid id, Guid? currentUserId = null, bool includeAppUser = false)
        {
            Order oOrder;

            if (includeAppUser)
                oOrder = await _unitOfWork.Order.GetByIdIncludeAppUserAsync(id);
            else
                oOrder = await _unitOfWork.Order.GetByIdAsync(id);

            if (oOrder == null)
                throw new NotFoundException(id, nameof(Order));

            if (currentUserId.HasValue)
            {
                var oClient = await _unitOfWork.Client.GetByUserIdAsync(currentUserId.Value);

                if (oClient != null)
                {
                    if (oClient.Id != oOrder.ClientId)
                        throw new UnauthorizedException();
                }
            }

            return oOrder;
        }

        public async Task<List<OrderArticleDetail>> GetOrderArticleDetailsById(Guid id)
        {
            return await _unitOfWork.Order.GetOrderArticleDetailsById(id);
        }

        public async Task<BusisnessResult> SetFinished(Guid orderId)
        {
            BusisnessResult oResult = new();

            var oOrderFromDb = await _unitOfWork.Order.GetByIdAsync(orderId);

            if(oOrderFromDb == null)
            {
                oResult.Message = "El pedido no existe.";

                return oResult;
            }

            if(oOrderFromDb.Status == OrderStatus.TERMINADO)
            {
                oResult.Message = "El pedido ya está marcado como terminado.";

                return oResult;
            }

            if (oOrderFromDb.Status == OrderStatus.CANCELADO)
            {
                oResult.Message = "El pedido no se puede marcar como terminado porque ha sido cancelado.";

                return oResult;
            }

            oOrderFromDb.Status = OrderStatus.TERMINADO;

            _unitOfWork.Order.Update(oOrderFromDb);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "El pedido se marcó como terminado.";

            return oResult;
        }

        public async Task<BusisnessResult> CancelMostRecent(Guid userId)
        {
            BusisnessResult oResult = new();

            var oClientFromDb = await _unitOfWork.Client.GetByUserIdAsync(userId);

            if (oClientFromDb == null)
            {
                oResult.Message = "El cliente no existe.";

                return oResult;
            }

            var oMostRecentOrder = _unitOfWork.Order.GetAllByUserId(userId)
                                                    .OrderByDescending(o => o.CreationDate)
                                                    .FirstOrDefault();

            if (oMostRecentOrder == null)
            {
                oResult.Message = "No hay pedidos recientes.";

                return oResult;
            }

            if (oMostRecentOrder.Status == OrderStatus.CANCELADO)
            {
                oResult.Message = $"El pedido {oMostRecentOrder.OrderNumber} ya está cancelado.";

                return oResult;
            }

            if (oMostRecentOrder.Status == OrderStatus.TERMINADO)
            {
                oResult.Message = $"El pedido {oMostRecentOrder.OrderNumber} no se puede cancelar porque ya ha sido terminado.";

                return oResult;
            }

            if (oMostRecentOrder.CreationDate.AddMinutes(5) < DateTimeUtility.NowCostaRica)
            {
                oResult.Message = "Sólo se pueden cancelar pedidos que tengan menos de 5 minutos de haber sido realizadas.";

                return oResult;
            }

            oMostRecentOrder.Status = OrderStatus.CANCELADO;

            oResult.Message = $"El pedido {oMostRecentOrder.OrderNumber} ha sido cancelado.";

            _unitOfWork.Order.Update(oMostRecentOrder);

            oResult.Success = await _unitOfWork.Save();

            return oResult;
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

            var oAddress = await _unitOfWork.Address.GetSelectedForUseByUserId(userId);

            if (oAddress == null)
            {
                oResult.Message = "Se debe de crear una dirección de envío.";

                oResult.ResultCode = 1;

                return oResult;
            }

            oOrder = new()
            {
                Id = new Guid(),
                ClientId = oCart.ClientId,
                CreationDate = DateTimeUtility.NowCostaRica,
                Status = OrderStatus.PENDIENTE,
                ShippingAddress = oAddress.Detail
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

            _unitOfWork.Cart.Create(new Cart
            {
                Id = new Guid(),
                OpeningDate = DateTimeUtility.NowCostaRica,
                ClientId = oCart.ClientId,
                Status = CartStatus.ABIERTO
            });

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

        public async Task<BusisnessResult> ChangeStatus(Guid orderId, OrderStatus status)
        {
            BusisnessResult oResult = new();

            var oOrderFromDb = await _unitOfWork.Order.GetByIdAsync(orderId);

            if(oOrderFromDb == null)
            {
                oResult.Message = "El pedido no existe.";

                return oResult;
            }

            oOrderFromDb.Status = status;

            _unitOfWork.Order.Update(oOrderFromDb);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "Se cambió el estado del pedido.";

            oResult.Data = new
            {
                id = oOrderFromDb.Id.ToString(),
                val = (int)oOrderFromDb.Status,
                text = oOrderFromDb.Status.ToString(),
                number = oOrderFromDb.OrderNumber
            };

            return oResult;
        }
    }
}
