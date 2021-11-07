using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.CustomEntities.Role
{
    public class RoleWithPermissions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EntityStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
