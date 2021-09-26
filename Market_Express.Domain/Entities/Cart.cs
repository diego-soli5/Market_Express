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

        public Guid ClientId { get; set; }
        public DateTime OpeningDate { get; set; }
        public string Status { get; set; }

        public Client Client { get; set; }
        public IEnumerable<CartDetail> CartDetails { get; set; }
    }
}
