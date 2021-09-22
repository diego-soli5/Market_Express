using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public partial class Rol : BaseEntity
    {
        public Rol()
        {
            RolPermisos = new HashSet<RolPermiso>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<RolPermiso> RolPermisos { get; set; }
    }
}
