using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Domain.QueryFilter.Order
{
    public interface IOrderQueryFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public OrderStatus? Status { get; set; }
    }
}
