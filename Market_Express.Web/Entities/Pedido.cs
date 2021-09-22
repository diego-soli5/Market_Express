using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Pedido
    {
        public Pedido()
        {
            PedidoDetalles = new HashSet<PedidoDetalle>();
        }

        public Guid Id { get; set; }
        public Guid IdCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
