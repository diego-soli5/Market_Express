using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            RolPermisos = new HashSet<RolePermission>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FecCreacion { get; set; }
        public string AdicionadoPor { get; set; }
        public string ModificadoPor { get; set; }

        public ICollection<RolePermission> RolPermisos { get; set; }
    }
}
