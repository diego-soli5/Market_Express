using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class PedidoDetalle
    {
        public Guid Id { get; set; }
        public Guid IdPedido { get; set; }
        public Guid IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Precio { get; set; }

        public virtual InventarioArticulo IdArticuloNavigation { get; set; }
        public virtual Pedido IdPedidoNavigation { get; set; }
    }
}
