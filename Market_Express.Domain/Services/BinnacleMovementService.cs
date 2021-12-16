using Market_Express.Domain.Options;
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
    public class BinnacleMovementService : BaseService, IBinnacleMovementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BinnacleMovementService(IUnitOfWork unitOfWork,
                                       IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SQLServerPagedList<BinnacleMovement>> GetAllPaginated(BinnacleMovementQueryFilter filters)
        {
            CheckPaginationFilters(filters);

            return await _unitOfWork.BinnacleMovement.GetAllPaginated(filters);
        }

        public async Task<List<BinnacleMovement>> GetAllForReport(BinnacleMovementQueryFilter filters)
        {
            return await _unitOfWork.BinnacleMovement.GetAllForReport(filters);
        }
    }
}
