using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Cliente : BaseEntity
    {
        public Cliente()
        {
            Carritos = new HashSet<Carrito>();
        }

        public Guid IdUsuario { get; set; }
        public string CodCliente { get; set; }
        public bool AutoSinc { get; set; }

        public Usuario IdUsuarioNavigation { get; set; }
        public ICollection<Carrito> Carritos { get; set; }
    }
}
