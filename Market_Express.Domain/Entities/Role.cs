using System;
using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Role : BaseEntity
    {
        public Role()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public IEnumerable<RolePermission> RolePermissions { get; set; }
    }
}
