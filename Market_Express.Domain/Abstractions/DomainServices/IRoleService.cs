using Market_Express.Domain.CustomEntities;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IRoleService
    {
        List<Role> GetAll();
        List<Permission> GetAllPermissions();
        Task<Permission> GetPermissionById(Guid id);
        Task<List<string>> GetAllPermissionTypes();
        Task<RoleWithPermissions> GetByIdWithPermissions(Guid id);
        Task<BusisnessResult> Edit(Role role, List<Guid> permissions, Guid currentUserId);
    }
}
