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

        public async Task<RoleWithPermissions> GetByIdWithPermissions(Guid id)
        {
            RoleWithPermissions oRoleWithPermissions = null;

            var oRole = await _unitOfWork.Role.GetByIdAsync(id);

            if(oRole != null)
            {
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
            }

            return oRoleWithPermissions;
        }
    }
}
