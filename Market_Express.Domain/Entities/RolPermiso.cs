using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class RolPermiso : BaseEntity
    {
        public Guid IdRol { get; set; }
        public Guid IdPermiso { get; set; }

        public Permiso IdPermisoNavigation { get; set; }
        public Rol IdRolNavigation { get; set; }
    }
}
