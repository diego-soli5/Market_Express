using System;

namespace Market_Express.Domain.QueryFilter.Report
{
    public class ReportClientQueryFilter : PaginationQueryFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
