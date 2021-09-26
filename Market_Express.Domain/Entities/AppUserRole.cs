using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class AppUserRole : BaseEntity
    {
        public Guid IdUsuario { get; set; }

        public AppUser IdUsuarioNavigation { get; set; }
    }
}
