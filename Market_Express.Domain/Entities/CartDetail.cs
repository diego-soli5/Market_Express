using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class CartDetail : BaseEntity
    {
        public Guid IdCarrito { get; set; }
        public Guid IdArticulo { get; set; }

        public Article Articulo { get; set; }
        public Cart Carrito { get; set; }
    }
}
