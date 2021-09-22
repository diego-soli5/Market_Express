using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class RolPermiso
    {
        public Guid Id { get; set; }
        public Guid IdRol { get; set; }
        public Guid IdPermiso { get; set; }

        public virtual Permiso IdPermisoNavigation { get; set; }
        public virtual Rol IdRolNavigation { get; set; }
    }
}
