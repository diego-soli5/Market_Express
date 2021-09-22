using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class CarritoDetalle : BaseEntity
    {
        public Guid IdCarrito { get; set; }
        public Guid IdArticulo { get; set; }

        public InventarioArticulo IdArticuloNavigation { get; set; }
        public Carrito IdCarritoNavigation { get; set; }
    }
}
