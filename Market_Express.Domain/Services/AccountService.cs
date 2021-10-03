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
        private readonly IBusisnessMailService _mailService;

        public AccountService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService,
                              IBusisnessMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _mailService = mailService;
        }

        public async Task<BusisnessResult> TryChangePassword(Guid userId, string currentPass, string newPass, string newPassConf)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrWhiteSpace(currentPass) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(newPassConf))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                return oResult;
            }

            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
            {
                oResult.Message = "Usuario no existe.";

                return oResult;
            }

            if (!_passwordService.Check(oUser.Password, currentPass))
            {
                oResult.Message = "La contraseña es incorrecta.";

                return oResult;
            }

            if (newPass.Trim() != newPassConf.Trim())
            {
                oResult.Message = "Las contraseñas no coinciden.";

                return oResult;
            }

            if (newPass.Trim().Length < 5)
            {
                oResult.Message = "La contraseña debe contener al menos 5 caracteres.";

                return oResult;
            }

            string ecnPass = _passwordService.Hash(newPass.Trim());

            oUser.Password = ecnPass;

            _unitOfWork.AppUser.Update(oUser);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "La contraseña ha cambiado";

            _mailService.SendMail("Market Express", "Su contraseña ha cambiado.", oUser.Email);

            return oResult;
        }

        public BusisnessResult TryAuthenticate(ref AppUser oUserRequest)
        {
            BusisnessResult oResult = new();

            string sRequestEmail = oUserRequest?.Email?.Trim();
            string sRequestPass = oUserRequest?.Password?.Trim();

            if (string.IsNullOrEmpty(sRequestEmail) || string.IsNullOrEmpty(sRequestPass))
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
