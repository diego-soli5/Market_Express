using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
using Market_Express.Domain.CustomEntities.Client;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Report;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class ReportService : BaseService, IReportService
    {
        #region ATTRIBUTES
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region CONSTRUCTOR
        public ReportService(IUnitOfWork unitOfWork,
                             IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region ORDERS
        public PagedList<Order> GetOrdersPaginated(ReportOrderQueryFilter filters)
        {
            var lstOrders = _unitOfWork.Order.GetAllIncludeAppUser();

            ApplyOrderFilters(ref lstOrders, filters);

            var pagedList = PagedList<Order>.Create(lstOrders, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedList;
        }

        public IQueryable<Order> GetOrdersForReport(ReportOrderQueryFilter filters)
        {
            var lstOrders = _unitOfWork.Order.GetAllIncludeAppUser();

            ApplyOrderFilters(ref lstOrders, filters);

            return lstOrders;
        }
        #endregion

        #region ARTICLES
        public async Task<SQLServerPagedList<ArticleForReport>> GetMostSoldArticlesPaginated(ReportArticleQueryFilter filters)
        {
            CheckPaginationFilters(filters);

            return await _unitOfWork.Report.GetMostSoldArticlesPaginated(filters);
        }

        public async Task<List<ArticleForReport>> GetMostSoldArticles(ReportArticleQueryFilter filters)
        {
            return await _unitOfWork.Report.GetMostSoldArticles(filters);
        }
        #endregion

        #region CLIENTS
        public async Task<SQLServerPagedList<ClientForReport>> GetClientsStatsPaginated(ReportClientQueryFilter filters)
        {
            CheckPaginationFilters(filters);

            return await _unitOfWork.Report.GetClientsStatsPaginated(filters);
        }

        public async Task<List<ClientForReport>> GetClientsStats(ReportClientQueryFilter filters)
        {
            return await _unitOfWork.Report.GetClientsStats(filters);
        }
        #endregion

        #region UTILITY METHODS
        private void ApplyOrderFilters(ref IQueryable<Order> orders, ReportOrderQueryFilter filters)
        {
            CheckPaginationFilters(filters);

            if (filters.ClientName != null)
                orders = orders.Where(o => o.Client.AppUser.Name.Trim().ToUpper() == filters.ClientName.Trim().ToUpper());

            if (filters.Status.HasValue)
                orders = orders.Where(o => o.Status == filters.Status.Value);

            if (filters.StartDate.HasValue)
            {

                orders = orders.Where(o => o.CreationDate.Date >= filters.StartDate.Value.Date);
            }

            if (filters.EndDate.HasValue)
                orders = orders.Where(o => o.CreationDate.Date <= filters.StartDate.Value.Date);

            orders = orders.OrderByDescending(o => o.CreationDate);
        }
        #endregion
    }
}
