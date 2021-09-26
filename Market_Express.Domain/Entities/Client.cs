using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            Carrito = new HashSet<Cart>();
        }

        public Guid IdUsuario { get; set; }
        public string CodCliente { get; set; }
        public bool AutoSinc { get; set; }

        public AppUser Usuario { get; set; }
        public ICollection<Cart> Carrito { get; set; }
    }
}
