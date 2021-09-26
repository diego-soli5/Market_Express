﻿using System.Collections.Generic;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<RolePermission> RolePermissions { get; set; }
    }
}
