using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Domain.QueryFilter.BinnacleMovement
{
    public class BinnacleMovementQueryFilter : PaginationQueryFilter
    {
        public MovementType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string User { get; set; }
        public bool IgnoreSystem { get; set; }
    }
}
