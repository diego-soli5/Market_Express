using System;

#nullable disable

namespace Market_Express.Domain.Entities
{
    public class AppUserRole : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Guid AppUserId { get; set; }

        public AppUser AppUser { get; set; }
        public Role Role { get; set; }
    }
}
