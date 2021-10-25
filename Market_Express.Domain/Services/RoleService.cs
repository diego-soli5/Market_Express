using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.CrossCutting.Utility;
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

        public async Task<BusisnessResult> Create(Role role, List<Guid> permissions, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            return oResult;
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

            var oRoleFromDb = await _unitOfWork.Role.GetByIdAsync(role.Id,nameof(Role.RolePermissions));

            if(oRoleFromDb == null)
            {
                oResult.Message = "El Rol no existe.";

                return oResult;
            }

            oRoleFromDb.Name = role.Name;
            oRoleFromDb.Description = role.Description;
            oRoleFromDb.ModifiedBy = currentUserId.ToString();
            oRoleFromDb.ModificationDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.RolePermission.Delete(oRoleFromDb.RolePermissions.ToList());

            foreach (var id in permissions)
            {
                var permissionToCheck = await _unitOfWork.Permission.GetByIdAsync(id);

                if(permissionToCheck == null)
                {
                    oResult.Message = $"El permiso {id} no existe.";

                    return oResult;
                }

                oRoleFromDb.RolePermissions.Add(new RolePermission
                {
                    PermissionId = id,
                    RoleId = oRoleFromDb.Id
                });
            }

            oResult.Message = "El Rol se modificó correctamente!";

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }
    }
}
