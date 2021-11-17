using AutoMapper;
using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Order;
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
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService,
                               IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(AdminOrderQueryFilter filters)
        {
            AdminOrderIndexViewModel oViewModel = new();

            oViewModel.OrderStats = await GetOrderStatsDTO();
            oViewModel.RecentOrders = await GetMostRecentOrderDTOList();

            var tplOrderDTOList = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrderDTOList.Item1;
            oViewModel.Metadata = tplOrderDTOList.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        #region UTILITY METHODS
        private async Task<List<RecentOrderDTO>> GetMostRecentOrderDTOList()
        {
            return (await _orderService.GetMostRecent())
                                       .Select(o => _mapper.Map<RecentOrderDTO>(o))
                                       .ToList();
        }

        private async Task<OrderStatsDTO> GetOrderStatsDTO()
        {
            return _mapper.Map<OrderStatsDTO>(await _orderService.GetOrderStats());
        }

        private (List<OrderDTO>,Metadata) GetOrderDTOList(AdminOrderQueryFilter filters)
        {
            var lstOrders = _orderService.GetAllPaginated(filters);

            var oMeta = Metadata.Create(lstOrders);

            return (lstOrders.Select(o => _mapper.Map<OrderDTO>(o)).ToList(), oMeta);
        }
        #endregion
    }
}
