using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class CarritoDetalle
    {
        public Guid Id { get; set; }
        public Guid IdCarrito { get; set; }
        public Guid IdArticulo { get; set; }

        public virtual InventarioArticulo IdArticuloNavigation { get; set; }
        public virtual Carrito IdCarritoNavigation { get; set; }
    }
}
