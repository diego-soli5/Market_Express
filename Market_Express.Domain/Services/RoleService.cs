using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Role;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork,
                           IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Role> GetAll(bool onlyActive = false)
        {
            var lstRoles = _unitOfWork.Role.GetAll();

            if (onlyActive)
                lstRoles = lstRoles.Where(role => role.Status == EntityStatus.ACTIVADO);

            lstRoles = lstRoles.OrderBy(r => r.Name);

            return lstRoles.ToList();
        }

        public async Task<List<Role>> GetAllByUserId(Guid id)
        {
            return await _unitOfWork.Role.GetAllByUserId(id);
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
                throw new NotFoundException(id, nameof(Permission));

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
                Status = oRole.Status,
                AddedBy = oRole.AddedBy,
                CreationDate = oRole.CreationDate,
                ModificationDate = oRole.ModificationDate,
                ModifiedBy = oRole.ModifiedBy
            };

            oRoleWithPermissions.Permissions = await _unitOfWork.Permission.GetAllByRoleId(id);


            return oRoleWithPermissions;
        }

        public async Task<(int, int)> GetUsersCountUsingARoleByRoleId(Guid id)
        {
            return await _unitOfWork.AppUserRole.GetUserCountUsingARole(id);
        }

        public async Task<BusisnessResult> Create(Role role, List<Guid> permissions, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if (!ValidateRoleFields(oResult, role))
                return oResult;

            if (permissions?.Count <= 0)
            {
                oResult.Message = "Se debe seleccionar al menos un permiso.";

                return oResult;
            }

            foreach (var id in permissions)
            {
                var permissionToCheck = await _unitOfWork.Permission.GetByIdAsync(id);

                if (permissionToCheck == null)
                {
                    oResult.Message = $"El permiso {id} no existe.";

                    return oResult;
                }

                if (role.RolePermissions == null)
                    role.RolePermissions = new List<RolePermission>();

                role.RolePermissions.Add(new RolePermission
                {
                    PermissionId = id,
                    RoleId = role.Id
                });
            }

            role.AddedBy = currentUserId.ToString();
            role.CreationDate = DateTimeUtility.NowCostaRica;
            role.Status = EntityStatus.ACTIVADO;

            _unitOfWork.Role.Create(role);

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            oResult.Message = "El rol se creó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> Edit(Role role, List<Guid> permissions, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if (!ValidateRoleFields(oResult, role))
                return oResult;

            if (permissions?.Count <= 0)
            {
                oResult.Message = "Se debe seleccionar al menos un permiso.";

                return oResult;
            }

            var oRoleFromDb = await _unitOfWork.Role.GetByIdAsync(role.Id, nameof(Role.RolePermissions));

            if (oRoleFromDb == null)
            {
                oResult.Message = "El Rol no existe.";

                return oResult;
            }

            if (oRoleFromDb.Status == EntityStatus.ACTIVADO && role.Status == EntityStatus.DESACTIVADO)
            {
                var oAppUserRoleToValidate = _unitOfWork.AppUserRole.GetFirstOrDefault(userRole => userRole.RoleId == oRoleFromDb.Id);

                if (oAppUserRoleToValidate != null)
                {
                    oResult.Message = "El rol no se puede desactivar porque está en uso.";

                    return oResult;
                }

                var lstRoles = _unitOfWork.Role.GetAll();

                if (lstRoles?.Count() <= 1 && oRoleFromDb.Status == EntityStatus.ACTIVADO)
                {
                    oResult.Message = "El rol no se puede desactivar porque es el único existente.";

                    return oResult;
                }
            }

            oRoleFromDb.Name = role.Name;
            oRoleFromDb.Description = role.Description;
            oRoleFromDb.Status = role.Status;
            oRoleFromDb.ModifiedBy = currentUserId.ToString();
            oRoleFromDb.ModificationDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.RolePermission.Delete(oRoleFromDb.RolePermissions.ToList());

            foreach (var id in permissions)
            {
                var permissionToCheck = await _unitOfWork.Permission.GetByIdAsync(id);

                if (permissionToCheck == null)
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

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            oResult.Message = "El rol se modificó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid roleId, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            var oRoleFromDb = await _unitOfWork.Role.GetByIdAsync(roleId);

            if (oRoleFromDb == null)
            {
                oResult.Message = "El rol no existe.";

                return oResult;
            }

            var oAppUserRoleToValidate = _unitOfWork.AppUserRole.GetFirstOrDefault(userRole => userRole.RoleId == oRoleFromDb.Id);

            if (oAppUserRoleToValidate != null && oRoleFromDb.Status == EntityStatus.ACTIVADO)
            {
                oResult.Message = "El rol no se puede desactivar porque está en uso.";

                return oResult;
            }

            var lstRoles = _unitOfWork.Role.GetAll();

            if (lstRoles?.Count() <= 1 && oRoleFromDb.Status == EntityStatus.ACTIVADO)
            {
                oResult.Message = "El rol no se puede desactivar porque es el único existente.";

                return oResult;
            }

            oRoleFromDb.ModifiedBy = currentUserId.ToString();
            oRoleFromDb.ModificationDate = DateTimeUtility.NowCostaRica;

            if (oRoleFromDb.Status == EntityStatus.DESACTIVADO)
            {
                oRoleFromDb.Status = EntityStatus.ACTIVADO;

                oResult.Message = "El rol se activó correctamente!";

                oResult.ResultCode = 1; //CSS set success
            }
            else
            {
                oRoleFromDb.Status = EntityStatus.DESACTIVADO;

                oResult.Message = "El rol se desactivó correctamente!";

                oResult.ResultCode = 0; //CSS set danger
            }

            _unitOfWork.Role.Update(oRoleFromDb);

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            return oResult;
        }

        #region UTILITY METHODS
        private bool ValidateRoleFields(BusisnessResult result, Role role)
        {
            if (string.IsNullOrEmpty(role.Name?.Trim()))
            {
                result.Message = "El campo nombre es obligario.";

                return false;
            }
            else if (role.Name.Length > 30)
            {
                result.Message = "El campo nombre no puede superar los 30 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(role.Description?.Trim()))
            {
                result.Message = "El campo descripción es obligatorio.";

                return false;
            }
            else if (role.Description.Length > 255)
            {
                result.Message = "El campo descripción no puede superar los 255 caracteres.";

                return false;
            }

            return true;
        }
        #endregion
    }
}
