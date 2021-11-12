using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Order
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Total { get; set; }
        public string ShippingAddress { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus Status { get; set; }
    }
}
