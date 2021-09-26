using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Article : BaseEntity
    {
        public Article()
        {
            CarritoDetalle = new HashSet<CartDetail>();
            PedidoDetalle = new HashSet<OrderDetail>();
        }

        public Guid? IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public bool AutoSinc { get; set; }
        public string Estado { get; set; }
        public DateTime FecCreacion { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }

        public ICollection<CartDetail> CarritoDetalle { get; set; }
        public ICollection<OrderDetail> PedidoDetalle { get; set; }
    }
}
