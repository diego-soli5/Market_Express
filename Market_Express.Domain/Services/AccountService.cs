using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.EntityConstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public AccountService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public BusisnessResult TryAuthenticate(ref AppUser oUserRequest)
        {
            BusisnessResult oResult = new();

            string sRequestEmail = oUserRequest?.Email?.Trim();
            string sRequestPass = oUserRequest?.Password?.Trim();

            if(string.IsNullOrEmpty(sRequestEmail)|| string.IsNullOrEmpty(sRequestPass))
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }

            var oUsuarioDB = _unitOfWork.AppUser
                .GetFirstOrDefault(x => x.Email.Trim() == sRequestEmail);

            if (oUsuarioDB == null)
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }  

            if (!_passwordService.Check(oUsuarioDB.Password, sRequestPass))
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }

            if (oUsuarioDB.Status == AppUserConstants.DESACTIVADO)
            {
                oResult.Message = "La cuenta está desactivada.";

                return oResult;
            }

            oUserRequest = oUsuarioDB;

            oResult.Success = true;

            return oResult;
        }

        public async Task<AppUser> GetUserInfo(Guid id)
        {
            return await _unitOfWork.AppUser.GetByIdAsync(id);
        }

        public async Task<List<Permission>> GetPermissionList(Guid id)
        {
            return await _unitOfWork.AppUser.GetPermissionList(id);
        }
    }
}
