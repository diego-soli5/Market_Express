using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Cliente
    {
        public Cliente()
        {
            Carritos = new HashSet<Carrito>();
        }

        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public string CodCliente { get; set; }
        public bool AutoSinc { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Carrito> Carritos { get; set; }
    }
}
