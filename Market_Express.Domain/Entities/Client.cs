using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Cart = new HashSet<Cart>();
        }

        public Guid UserId { get; set; }
        public string ClientCode { get; set; }
        public bool AutoSync { get; set; }

        public AppUser AppUser { get; set; }
        public IEnumerable<Cart> Cart { get; set; }
    }
}
