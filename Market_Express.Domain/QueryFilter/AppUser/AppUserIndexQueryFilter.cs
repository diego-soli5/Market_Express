using Market_Express.Domain.Enumerations;

namespace Market_Express.Domain.QueryFilter.AppUser
{
    public class AppUserIndexQueryFilter
    {
        public string Name { get; set; }
        public string Identification { get; set; }
        public EntityStatus? Status { get; set; }
        public AppUserType? Type { get; set; }
    }
}
