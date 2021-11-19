using System;
using System.Collections.Generic;
using System.Linq;
using Market_Express.Domain.Entities;

namespace Market_Express.Domain.CustomEntities.Pagination
{
    public class PagedList<TEntity> : List<TEntity> where TEntity : BaseEntity
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null;

        public PagedList(IQueryable<TEntity> entities, int totalCount, int currentPage, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(TotalCount / (decimal)PageSize);

            AddRange(entities);
        }

        public static PagedList<TEntity> Create(IQueryable<TEntity> entities, int pageNumber, int pageSize)
        {
            var count = entities.Count();
            var entitiesPaginated = entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedList<TEntity>(entitiesPaginated, count, pageNumber, pageSize);
        }
    }
}
