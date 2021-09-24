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

        public BusisnessResult TryAuthenticate(Usuario usuarioRequest)
        {
            BusisnessResult result = new();

            var oUsuarioDB = _unitOfWork.Usuario
                .GetFirstOrDefault(x => x.Email.Trim() == usuarioRequest.Email.Trim());

            if (oUsuarioDB == null)
            {
                result.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return result;
            }  

            if (!_passwordService.Check(oUsuarioDB.Clave, usuarioRequest.Clave))
            {
                result.Message = "Correo Electrónico y/o contraseña incorrectos.";

                return result;
            }

            if (oUsuarioDB.Estado == UsuarioConstants.DESACTIVADO)
            {
                result.Message = "La cuenta está desactivada.";

                return result;
            }

            usuarioRequest = oUsuarioDB;

            return result;
        }

        public async Task<List<Permiso>> GetPermisos(Guid id)
        {
            return await _unitOfWork.Usuario.GetPermisosAsync(id);
        }
    }
}
