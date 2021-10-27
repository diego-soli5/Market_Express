using Market_Express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IAppUserRoleRepository : IGenericRepository<AppUserRole>
    {
        Task<int> GetUserCountUsingARole(Guid roleId);
    }
}
