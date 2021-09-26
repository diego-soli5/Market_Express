using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Guid IdPedido { get; set; }
        public Guid IdArticulo { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Precio { get; set; }

        public Article IdArticuloNavigation { get; set; }
        public Order IdPedidoNavigation { get; set; }
    }
}
