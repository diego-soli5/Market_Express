using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            AppUserRoles = new HashSet<AppUserRole>();
            RolePermissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
