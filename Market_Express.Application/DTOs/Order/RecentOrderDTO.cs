using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Order
{
    public class RecentOrderDTO
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
        public string MostRequestedArticleImage { get; set; }
    }
}
