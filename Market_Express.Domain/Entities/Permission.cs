using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public Permission()
        {
            RolPermisos = new HashSet<RolePermission>();
        }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<RolePermission> RolPermisos { get; set; }
    }
}
