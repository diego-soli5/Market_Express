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

namespace Market_Express.Web.Areas.Client.Controllers
{
    [Area("Client")]
    [Authorize]
    public class OrderController : BaseController
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
        public async Task<IActionResult> Index(MyOrdersQueryFilter filters)
        {
            MyOrdersViewModel oViewModel = new();

            oViewModel.OrderStats = await GetOrderStatsDTO();
            oViewModel.RecentOrders = await GetRecentOrderDTOList();

            var tplOrders = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrders.Item1;
            oViewModel.Metadata = tplOrders.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        [HttpGet]
        [Route("/Client/Order/Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            OrderClientDetailViewModel oViewModel = new();

            oViewModel.Order = _mapper.Map<OrderDTO>(await _orderService.GetById(id));
            oViewModel.Details = (await _orderService.GetOrderArticleDetailsById(id))
                                                     .Select(o => _mapper.Map<OrderArticleDetailDTO>(o))
                                                     .ToList();
            
            return View(oViewModel);
        }

        [HttpGet]
        public IActionResult GetMyOrdersTable(MyOrdersQueryFilter filters)
        {
            MyOrdersViewModel oViewModel = new();

            var tplOrders = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrders.Item1;
            oViewModel.Metadata = tplOrders.Item2;
            oViewModel.Filters = filters;

            return PartialView("_MyOrdersTablePartial", oViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecentOrders()
        {
            return PartialView("_MyOrdersRecentPartial", await GetRecentOrderDTOList());
        }

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            return PartialView("_MyOrdersStatsPartial", await GetOrderStatsDTO());
        }

        #region API CALLS
        [HttpPost]
        public async Task<IActionResult> CancelRecent()
        {
            var oResult = await _orderService.CancelMostRecent(CurrentUserId);

            return Ok(oResult);
        }
        #endregion

        #region UTILITY METHODS
        private async Task<List<RecentOrderDTO>> GetRecentOrderDTOList()
        {
            return (await _orderService.GetRecentOrdersByUserId(CurrentUserId))
                                       .Select(o => _mapper.Map<RecentOrderDTO>(o))
                                       .ToList();
        }

        private (List<OrderDTO>, Metadata) GetOrderDTOList(MyOrdersQueryFilter filters)
        {
            List<OrderDTO> lstOrderDTO = new();

            var pagedOrders = _orderService.GetAllByUserId(CurrentUserId, filters);
            var oMeta = Metadata.Create(pagedOrders);

            lstOrderDTO.AddRange(pagedOrders.Select(o => _mapper.Map<OrderDTO>(o)).ToList());

            return (lstOrderDTO, oMeta);
        }

        private async Task<OrderStatsDTO> GetOrderStatsDTO()
        {
            return _mapper.Map<OrderStatsDTO>(await _orderService.GetOrderStatsByUserId(CurrentUserId));
        }
        #endregion
    }
}
