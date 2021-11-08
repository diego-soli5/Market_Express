using Market_Express.Domain.Enumerations;
using System;

namespace Market_Express.Application.DTOs.Cart
{
    public class CartBillingDetailsDTO
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public DateTime OpeningDate { get; set; }
        public CartStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }

        public decimal Total => SubTotal - Discount;
    }
}
