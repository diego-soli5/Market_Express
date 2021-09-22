using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class RolPermiso : BaseEntity
    {
        public Guid IdRol { get; set; }
        public Guid IdPermiso { get; set; }

        public virtual Permiso IdPermisoNavigation { get; set; }
        public virtual Rol IdRolNavigation { get; set; }
    }
}
