using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.Report;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Market_Express.Domain.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public ReportService(IUnitOfWork unitOfWork,
                             IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public PagedList<Order> GetOrdersPaginated(ReportOrderQueryFilter filters)
        {
            var lstOrders = _unitOfWork.Order.GetAllIncludeAppUser();

            ApplyOrderFilters(ref lstOrders, filters);

            var pagedList = PagedList<Order>.Create(lstOrders, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedList;
        }

        public IEnumerable<Order> GetOrdersForReport(ReportOrderQueryFilter filters)
        {
            var lstOrders = _unitOfWork.Order.GetAllIncludeAppUser();

            ApplyOrderFilters(ref lstOrders, filters);

            return lstOrders;
        }

        #region UTILITY METHODS
        private void ApplyOrderFilters(ref IQueryable<Order> orders, ReportOrderQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

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
