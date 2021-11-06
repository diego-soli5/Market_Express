using Market_Express.Domain.CustomEntities;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IRoleService
    {
        List<Role> GetAll(bool onlyActive = false);
        List<Permission> GetAllPermissions();
        Task<Permission> GetPermissionById(Guid id);
        Task<List<string>> GetAllPermissionTypes();
        Task<List<Role>> GetAllByUserId(Guid id);
        Task<RoleWithPermissions> GetByIdWithPermissions(Guid id);
        Task<BusisnessResult> Create(Role role, List<Guid> permissions, Guid currentUserId);
        Task<BusisnessResult> Edit(Role role, List<Guid> permissions, Guid currentUserId);
        Task<BusisnessResult> ChangeStatus(Guid roleId, Guid currentUserId);
        Task<(int, int)> GetUsersCountUsingARoleByRoleId(Guid id);
    }
}
