using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class CarritoDetalle : BaseEntity
    {
        public Guid IdCarrito { get; set; }
        public Guid IdArticulo { get; set; }

        public InventarioArticulo Articulo { get; set; }
        public Carrito Carrito { get; set; }
    }
}
