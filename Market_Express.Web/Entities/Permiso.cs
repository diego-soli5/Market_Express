using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Web.Entities
{
    public partial class Permiso
    {
        public Permiso()
        {
            RolPermisos = new HashSet<RolPermiso>();
        }

        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<RolPermiso> RolPermisos { get; set; }
    }
}
