using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Addresses = new HashSet<Address>();
            Carts = new HashSet<Cart>();
            TbOrders = new HashSet<Order>();
        }

        public Client(bool autoSync, string clientCode)
        {
            AutoSync = autoSync;
            ClientCode = clientCode;
        }

        public Guid AppUserId { get; set; }
        public string ClientCode { get; set; }
        public bool AutoSync { get; set; }

        public AppUser AppUser { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> TbOrders { get; set; }
    }
}
