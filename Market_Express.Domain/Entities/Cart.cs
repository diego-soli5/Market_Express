using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Cart()
        {
            CartDetails = new HashSet<CartDetail>();
        }

        public Cart(Guid id, Guid clientId, DateTime openingDate, CartStatus status)
        {
            Id = id;
            ClientId = clientId;
            OpeningDate = openingDate;
            Status = status;
        }

        public Guid ClientId { get; set; }
        public DateTime OpeningDate { get; set; }
        public CartStatus Status { get; set; }

        public Client Client { get; set; }
        public ICollection<CartDetail> CartDetails { get; set; }
    }
}
