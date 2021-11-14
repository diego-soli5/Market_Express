using System;

namespace Market_Express.Domain.QueryFilter.BinnacleMovement
{
    public class BinnacleMovementQueryFilter : PaginationQueryFilter
    {
        public string Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string User { get; set; }
        public bool IgnoreSystem { get; set; }
    }
}
