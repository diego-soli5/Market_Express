using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class UsuarioRol : BaseEntity
    {
        public Guid IdUsuario { get; set; }

        public Usuario IdUsuarioNavigation { get; set; }
    }
}
