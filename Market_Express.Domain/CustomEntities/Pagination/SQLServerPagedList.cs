using Market_Express.Domain.Entities;
using System.Collections.Generic;

namespace Market_Express.Domain.CustomEntities.Pagination
{
    public class SQLServerPagedList<TEntity> : List<TEntity> where TEntity : BaseEntity
    {
        public SQLServerPagedList(List<TEntity> entities, int pageNumber, int pageSize, int totalPages, int totalCount)
        {
            AddRange(entities);

            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalCount = totalCount;
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;
    }
}
