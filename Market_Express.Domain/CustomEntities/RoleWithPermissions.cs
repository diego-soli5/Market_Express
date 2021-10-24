using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Market_Express.Domain.CustomEntities
{
    public class RoleWithPermissions
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string AddedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
