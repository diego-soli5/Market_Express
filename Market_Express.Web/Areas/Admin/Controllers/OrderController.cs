using AutoMapper;
using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Order;
using Market_Express.Web.Controllers;
using Market_Express.Web.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "ORD_MAN_GEN")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
                               IAppUserService appUserService,
                               IMapper mapper)
        {
            _orderService = orderService;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AdminOrderQueryFilter filters)
        {
            AdminOrderIndexViewModel oViewModel = new();

            oViewModel.OrderStats = await GetOrderStatsDTO();
            oViewModel.RecentPendingOrders = await GetRecentPendingOrderDTOList();

            var tplOrderDTOList = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrderDTOList.Item1;
            oViewModel.Metadata = tplOrderDTOList.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        [HttpGet]
        [Route("/Admin/Order/View/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            OrderAdminDetailsViewModel oViewModel = new();

            oViewModel.Order = _mapper.Map<OrderDTO>(await _orderService.GetById(id, null, true));
            oViewModel.Details = (await _orderService.GetOrderArticleDetailsById(id))
                                                     .Select(o => _mapper.Map<OrderArticleDetailDTO>(o))
                                                     .ToList();

            return View(oViewModel);
        }

        public async Task<IActionResult> GetPending()
        {
            return PartialView("_RecentPendingOrdersPartial", await GetRecentPendingOrderDTOList());
        }

        public async Task<IActionResult> GetStats()
        {
            return PartialView("_OrderStatsPartial", await GetOrderStatsDTO());
        }

        public IActionResult GetTable(AdminOrderQueryFilter filters)
        {
            AdminOrderIndexViewModel oViewModel = new();

            var tplOrderDTOList = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrderDTOList.Item1;
            oViewModel.Metadata = tplOrderDTOList.Item2;
            oViewModel.Filters = filters;

            return PartialView("_OrdersTablePartial", oViewModel);
        }

        public IActionResult SearchClient(string query)
        {
            var lstResult = _appUserService.SearchNames(query);

            return Ok(lstResult.Select(s => new { Name = s }));
        }

        #region UTILITY METHODS
        private async Task<List<RecentOrderDTO>> GetRecentPendingOrderDTOList()
        {
            return (await _orderService.GetMostRecent())
                                       .Select(o => _mapper.Map<RecentOrderDTO>(o))
                                       .ToList();
        }

        private async Task<OrderStatsDTO> GetOrderStatsDTO()
        {
            return _mapper.Map<OrderStatsDTO>(await _orderService.GetOrderStats());
        }

        private (List<OrderDTO>, Metadata) GetOrderDTOList(AdminOrderQueryFilter filters)
        {
            var lstOrders = _orderService.GetAllPaginated(filters);

            var oMeta = Metadata.Create(lstOrders);

            return (lstOrders.Select(o => _mapper.Map<OrderDTO>(o)).ToList(), oMeta);
        }
        #endregion
    }
}
