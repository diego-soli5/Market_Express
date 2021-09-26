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
            string sRequestPass = oUserRequest?.Clave?.Trim();

            if(string.IsNullOrEmpty(sRequestEmail)|| string.IsNullOrEmpty(sRequestPass))
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }

            var oUsuarioDB = _unitOfWork.Usuario
                .GetFirstOrDefault(x => x.Email.Trim() == sRequestEmail);

            if (oUsuarioDB == null)
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }  

            if (!_passwordService.Check(oUsuarioDB.Clave, sRequestPass))
            {
                oResult.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return oResult;
            }

            if (oUsuarioDB.Estado == UsuarioConstants.DESACTIVADO)
            {
                oResult.Message = "La cuenta está desactivada.";

                return oResult;
            }

            oUserRequest = oUsuarioDB;

            oResult.Success = true;

            return oResult;
        }

        public async Task<List<Permission>> GetPermisos(Guid id)
        {
            return await _unitOfWork.Usuario.GetPermisosAsync(id);
        }
    }
}
