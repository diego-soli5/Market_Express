using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Cart()
        {
            CarritoDetalle = new HashSet<CartDetail>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FecApertura { get; set; }
        public string Estado { get; set; }

        public Client Cliente { get; set; }
        public ICollection<CartDetail> CarritoDetalle { get; set; }
    }
}
