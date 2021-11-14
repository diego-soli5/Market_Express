using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.BinnacleAccess;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class BinnacleAccessService : IBinnacleAccessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public BinnacleAccessService(IUnitOfWork unitOfWork,
                                     IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public PagedList<BinnacleAccess> GetAll(BinnacleAccessQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            var lstBinnacleAccess = _unitOfWork.BinnacleAccess.GetAll(nameof(BinnacleAccess.AppUser));

            if (filters.StartDate != null)
                lstBinnacleAccess = lstBinnacleAccess.Where(b => DateTimeUtility.Truncate(b.EntryDate) >= DateTimeUtility.Truncate(filters.StartDate.Value));

            if (filters.EndDate != null)
                lstBinnacleAccess = lstBinnacleAccess.Where(b => b.ExitDate.HasValue && DateTimeUtility.Truncate(b.ExitDate.Value) <= DateTimeUtility.Truncate(filters.EndDate.Value));

            if (filters.User != null)
                lstBinnacleAccess = lstBinnacleAccess.Where(b => b.AppUser.Name.ToUpper() == filters.User.ToUpper());

            lstBinnacleAccess = lstBinnacleAccess.OrderByDescending(b => b.EntryDate).AsEnumerable();

            var pagedList = PagedList<BinnacleAccess>.Create(lstBinnacleAccess, filters.PageNumber.Value, filters.PageSize.Value);

            return pagedList;
        }

        public async Task RegisterAccess(Guid userId)
        {
            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
                return;

            BinnacleAccess oBinnacleAccess = new()
            {
                Id = new Guid(),
                AppUserId = userId,
                EntryDate = DateTimeUtility.NowCostaRica,
                ExitDate = null
            };

            _unitOfWork.BinnacleAccess.Create(oBinnacleAccess);

            await _unitOfWork.Save();
        }

        public async Task RegisterExit(Guid userId)
        {
            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
                return;

            var oBinnacleAccess = await _unitOfWork.BinnacleAccess.GetLastAccessByUserId(userId);

            if (oBinnacleAccess == null)
                return;

            oBinnacleAccess.ExitDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.BinnacleAccess.Update(oBinnacleAccess);

            await _unitOfWork.Save();
        }
    }
}
