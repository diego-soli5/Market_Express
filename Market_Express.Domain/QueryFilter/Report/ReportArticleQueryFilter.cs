using System;

namespace Market_Express.Domain.QueryFilter.Report
{
    public class ReportArticleQueryFilter : PaginationQueryFilter
    {
        public Guid? CategoryId { get; set; }
        public string Description { get; set; }
        public int? MinPrice{ get; set; }
        public int? MaxPrice{ get; set; }
    }
}
