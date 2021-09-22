using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class UsuarioRol : BaseEntity
    {
        public Guid IdUsuario { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
