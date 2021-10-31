namespace Market_Express.Domain.QueryFilter
{
    public abstract class PaginationQueryFilter
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
}
