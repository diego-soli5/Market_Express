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

        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Role(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
