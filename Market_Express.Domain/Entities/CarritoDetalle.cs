using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class CarritoDetalle : BaseEntity
    {
        public Guid IdCarrito { get; set; }
        public Guid IdArticulo { get; set; }

        public virtual InventarioArticulo IdArticuloNavigation { get; set; }
        public virtual Carrito IdCarritoNavigation { get; set; }
    }
}
