using AutoMapper;
using Market_Express.Application.DTOs.Order;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.QueryFilter.Report;
using Market_Express.Web.ViewModels.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Rotativa.AspNetCore;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "REP_USE_GEN")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService,
                                IMapper mapper)
        {
            _reportService = reportService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Order(ReportOrderQueryFilter filters)
        {
            ReportOrderViewModel oViewModel = new();

            var tplOrderDTOList = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrderDTOList.Item1;
            oViewModel.Metadata = tplOrderDTOList.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        [HttpGet]
        public IActionResult OrderReport(ReportOrderQueryFilter filters)
        {
            ReportOrderViewModel oViewModel = new();

            oViewModel.Filters = filters;
            oViewModel.Orders = _reportService.GetOrdersForReport(filters)
                                              .Select(o => _mapper.Map<OrderDTO>(o))
                                              .ToList();

            return new ViewAsPdf(oViewModel);
        }

        [HttpGet]
        public IActionResult GetOrderTable(ReportOrderQueryFilter filters)
        {
            ReportOrderViewModel oViewModel = new();

            var tplOrderDTOList = GetOrderDTOList(filters);

            oViewModel.Orders = tplOrderDTOList.Item1;
            oViewModel.Metadata = tplOrderDTOList.Item2;

            return PartialView("_OrderReportTablePartial", oViewModel);
        }

        [HttpGet]
        public IActionResult Article(ReportClientQueryFilter filters)
        {
            ReportArticleViewModel oViewModel = new();

            return View(oViewModel);
        }

        [HttpGet]
        public IActionResult Client(ReportClientQueryFilter filters)
        {
            ReportClientViewModel oViewModel = new();

            return View(oViewModel);
        }

        #region UTILITY METHODS
        public (List<OrderDTO>,Metadata) GetOrderDTOList(ReportOrderQueryFilter filters)
        {
            var lstOrders = _reportService.GetOrdersPaginated(filters);

            var oMeta = Metadata.Create(lstOrders);

            return (lstOrders.Select(o => _mapper.Map<OrderDTO>(o)).ToList(), oMeta);
        }
        #endregion
    }
}
