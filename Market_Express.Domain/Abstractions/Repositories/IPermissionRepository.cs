using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<Permission>> GetAllByRoleId(Guid id);
        Task<List<string>> GetAllTypes();
    }
}
