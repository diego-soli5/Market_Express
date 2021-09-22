using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class Pedido : BaseEntity
    {
        public Pedido()
        {
            PedidoDetalles = new HashSet<PedidoDetalle>();
        }

        public Guid IdCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
