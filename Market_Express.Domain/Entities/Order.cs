using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            PedidoDetalles = new HashSet<OrderDetail>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FecCreacion { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public ICollection<OrderDetail> PedidoDetalles { get; set; }
    }
}
