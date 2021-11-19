using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Article;
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
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork,
                             IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SQLServerPagedList<ArticleForReport>> GetMostSoldArticlesPaginated(ReportArticleQueryFilter filters)
        {
            CheckPaginationFilters(filters);

            return await _unitOfWork.Report.GetMostSoldArticlesPaginated(filters);
        }

        public async Task<List<ArticleForReport>> GetMostSoldArticles(ReportArticleQueryFilter filters)
        {
            return await _unitOfWork.Report.GetMostSoldArticles(filters);
        }

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
