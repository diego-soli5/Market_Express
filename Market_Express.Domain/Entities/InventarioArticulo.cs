using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class InventarioArticulo : BaseEntity
    {
        public InventarioArticulo()
        {
            CarritoDetalles = new HashSet<CarritoDetalle>();
            PedidoDetalles = new HashSet<PedidoDetalle>();
        }

        public Guid? IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public string CodigoBarras { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public bool AutoSinc { get; set; }
        public string Estado { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }

        public ICollection<CarritoDetalle> CarritoDetalles { get; set; }
        public ICollection<PedidoDetalle> PedidoDetalles { get; set; }
    }
}
