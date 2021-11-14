using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Pagination;
using Market_Express.Domain.Entities;
using Market_Express.Domain.QueryFilter.BinnacleMovement;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class BinnacleMovementService : IBinnacleMovementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public BinnacleMovementService(IUnitOfWork unitOfWork,
                                       IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task<SQLServerPagedList<BinnacleMovement>> GetAllPaginated(BinnacleMovementQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;

            return await _unitOfWork.BinnacleMovement.GetAllPaginated(filters);
        }

        public async Task<List<BinnacleMovement>> GetAllForReport(BinnacleMovementQueryFilter filters)
        {
            return await _unitOfWork.BinnacleMovement.GetAllForReport(filters);
        }
    }
}
