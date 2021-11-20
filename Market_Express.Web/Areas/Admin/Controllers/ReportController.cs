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
using Market_Express.Application.DTOs.Article;
using System.Threading.Tasks;
using Market_Express.Application.DTOs.Category;
using Market_Express.Application.DTOs.Client;

namespace Market_Express.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Authorize(Roles = "REP_USE_GEN")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService,
                                ICategoryService categoryService,
                                IMapper mapper)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        #region ORDER REPORT ENDPOINTS
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
        #endregion

        #region ARTICLE REPORT ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> Article(ReportArticleQueryFilter filters)
        {
            ReportArticleViewModel oViewModel = new();

            var tplArticleForReportDTOList = await GetArticleDTOList(filters);
            
            oViewModel.Articles = tplArticleForReportDTOList.Item1;
            oViewModel.Metadata = tplArticleForReportDTOList.Item2;
            oViewModel.Filters = filters;
            oViewModel.Categories = _categoryService.GetAll()
                                                    .Select(cat => _mapper.Map<CategoryDTO>(cat))
                                                    .ToList();

            return View(oViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ArticleReport(ReportArticleQueryFilter filters)
        {
            ReportArticleViewModel oViewModel = new();

            oViewModel.Articles = (await _reportService.GetMostSoldArticles(filters))
                                                       .Select(a => _mapper.Map<ArticleForReportDTO>(a))
                                                       .ToList();
            oViewModel.Filters = filters;

            if (filters.CategoryId.HasValue)
                oViewModel.CategoryName = (await _categoryService.GetById(filters.CategoryId.Value))?.Name;

            return new ViewAsPdf(oViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticleTable(ReportArticleQueryFilter filters)
        {
            ReportArticleViewModel oViewModel = new();

            var tplArticleForReportDTOList = await GetArticleDTOList(filters);

            oViewModel.Articles = tplArticleForReportDTOList.Item1;
            oViewModel.Metadata = tplArticleForReportDTOList.Item2;
            oViewModel.Filters = filters;

            return PartialView("_ArticleReportTablePartial", oViewModel);
        }
        #endregion

        #region CLIENT REPORT ENDPOINTS
        [HttpGet]
        public async Task<IActionResult> Client(ReportClientQueryFilter filters)
        {
            ReportClientViewModel oViewModel = new();

            var tplClientDTOList = await GetClientDTOList(filters);

            oViewModel.Clients = tplClientDTOList.Item1;
            oViewModel.Metadata = tplClientDTOList.Item2;
            oViewModel.Filters = filters;

            return View(oViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ClientReport(ReportClientQueryFilter filters)
        {
            ReportClientViewModel oViewModel = new();

            oViewModel.Filters = filters;
            oViewModel.Clients = (await _reportService.GetClientsStats(filters))
                                                      .Select(c => _mapper.Map<ClientForReportDTO>(c))
                                                      .ToList();

            return new ViewAsPdf(oViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetClientTable(ReportClientQueryFilter filters)
        {
            ReportClientViewModel oViewModel = new();

            var tplClientDTOList = await GetClientDTOList(filters);

            oViewModel.Clients = tplClientDTOList.Item1;
            oViewModel.Metadata = tplClientDTOList.Item2;
            oViewModel.Filters = filters;

            return PartialView("_ClientReportTablePartial", oViewModel);
        }
        #endregion

        #region UTILITY METHODS
        public async Task<(List<ArticleForReportDTO>, Metadata)> GetArticleDTOList(ReportArticleQueryFilter filters)
        {
            var lstArticle = await _reportService.GetMostSoldArticlesPaginated(filters);

            var oMeta = Metadata.Create(lstArticle);

            return (lstArticle.Select(o => _mapper.Map<ArticleForReportDTO>(o)).ToList(), oMeta);
        }

        public (List<OrderDTO>, Metadata) GetOrderDTOList(ReportOrderQueryFilter filters)
        {
            var lstOrders = _reportService.GetOrdersPaginated(filters);

            var oMeta = Metadata.Create(lstOrders);

            return (lstOrders.Select(o => _mapper.Map<OrderDTO>(o)).ToList(), oMeta);
        }

        public async Task<(List<ClientForReportDTO>, Metadata)> GetClientDTOList(ReportClientQueryFilter filters)
        {
            var lstClient = await _reportService.GetClientsStatsPaginated(filters);

            var oMeta = Metadata.Create(lstClient);

            return (lstClient.Select(o => _mapper.Map<ClientForReportDTO>(o)).ToList(), oMeta);
        }
        #endregion
    }
}
