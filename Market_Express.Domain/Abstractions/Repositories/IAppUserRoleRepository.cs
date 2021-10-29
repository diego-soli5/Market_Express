using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IAppUserRoleRepository : IGenericRepository<AppUserRole>
    {
        Task<(int, int)> GetUserCountUsingARole(Guid roleId);
        IEnumerable<AppUserRole> GetAllByUserId(Guid id);
    }
}
