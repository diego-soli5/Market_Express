using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public Guid IdRol { get; set; }
        public Guid IdPermiso { get; set; }

        public Permission IdPermisoNavigation { get; set; }
        public Role IdRolNavigation { get; set; }
    }
}
