using Market_Express.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IAppUserRoleRepository : IGenericRepository<AppUserRole>
    {
        Task<(int, int)> GetUserCountUsingARole(Guid roleId);
        IQueryable<AppUserRole> GetAllByUserId(Guid id);
    }
}
