using Market_Express.Domain.Options;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.QueryFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class BaseService
    {
        private readonly PaginationOptions _paginationOptions;

        public BaseService(IOptions<PaginationOptions> paginationOptions)
        {
            _paginationOptions = paginationOptions.Value;
        }

        protected async Task<bool> SaveWithTransaction(IUnitOfWork unitOfWork)
        {
            try
            {
                await unitOfWork.BeginTransactionAsync();

                bool ok = await unitOfWork.Save();

                await unitOfWork.CommitTransactionAsync();

                return ok;
            }
            catch (Exception ex)
            {
                await unitOfWork.RollBackAsync();

                throw ex;
            }
        }

        protected bool IsValidImage(IFormFile image)
        {
            var validImageTypes = new[] {

                    "image/png",
                    "image/jpg",
                    "image/jpeg"
                };

            if (!validImageTypes.Any(x => x == image.ContentType))
            {
                return false;
            }

            return true;
        }

        protected void CheckPaginationFilters(PaginationQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber != null && filters.PageNumber > 0 ? filters.PageNumber.Value : _paginationOptions.DefaultPageNumber;
            filters.PageSize = filters.PageSize != null && filters.PageSize > 0 ? filters.PageSize.Value : _paginationOptions.DefaultPageSize;
        }
    }
}
