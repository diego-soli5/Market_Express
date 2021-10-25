using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities;
using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Role> GetAll()
        {
            return _unitOfWork.Role.GetAll().ToList();
        }

        public List<Permission> GetAllPermissions()
        {
            return _unitOfWork.Permission.GetAll()
                                         .OrderBy(p => p.Type)
                                         .ToList();
        }

        public async Task<Permission> GetPermissionById(Guid id)
        {
            var oPermission = await _unitOfWork.Permission.GetByIdAsync(id);

            if (oPermission == null)
                throw new NotFoundException(id,nameof(Permission));

            return oPermission;
        }

        public async Task<List<string>> GetAllPermissionTypes()
        {
            return await _unitOfWork.Permission.GetAllTypes();
        }

        public async Task<RoleWithPermissions> GetByIdWithPermissions(Guid id)
        {
            RoleWithPermissions oRoleWithPermissions = null;

            var oRole = await _unitOfWork.Role.GetByIdAsync(id);

            if (oRole == null)
                throw new NotFoundException(id, nameof(Role));

            oRoleWithPermissions = new()
            {
                Id = oRole.Id,
                Name = oRole.Name,
                Description = oRole.Description,
                AddedBy = oRole.AddedBy,
                CreationDate = oRole.CreationDate,
                ModificationDate = oRole.ModificationDate,
                ModifiedBy = oRole.ModifiedBy
            };

            oRoleWithPermissions.Permissions = await _unitOfWork.Permission.GetAllByRoleId(id);


            return oRoleWithPermissions;
        }

        public async Task<BusisnessResult> Edit(Role role, List<Guid> permissions, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if(string.IsNullOrEmpty(role.Name?.Trim()) || string.IsNullOrEmpty(role.Description?.Trim()))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                return oResult;
            }

            if(permissions?.Count <= 0)
            {
                oResult.Message = "Se debe seleccionar al menos un permiso.";

                return oResult;
            }

            return oResult;
        }
    }
}
