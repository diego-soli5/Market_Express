using Market_Express.Domain.Entities;

namespace Market_Express.Domain.CustomEntities.Pagination
{
    public class Metadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int? NextPageNumber { get; set; }
        public int? PreviousPageNumber { get; set; }

        public static Metadata Create<TEntity>(PagedList<TEntity> pagedList)
            where TEntity : BaseEntity
        {
            return new Metadata
            {
                TotalCount = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                HasNextPage = pagedList.HasNextPage,
                HasPreviousPage = pagedList.HasPreviousPage,
                NextPageNumber = pagedList.NextPageNumber,
                PreviousPageNumber = pagedList.PreviousPageNumber
            };
        }

        public static Metadata Create<TEntity>(SQLServerPagedList<TEntity> pagedList)
           where TEntity : BaseEntity
        {
            return new Metadata
            {
                TotalCount = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                HasNextPage = pagedList.HasNextPage,
                HasPreviousPage = pagedList.HasPreviousPage,
                NextPageNumber = pagedList.NextPageNumber,
                PreviousPageNumber = pagedList.PreviousPageNumber
            };
        }
    }
}
