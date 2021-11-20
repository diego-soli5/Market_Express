using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IBusisnessMailService _mailService;

        public AccountService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService,
                              IBusisnessMailService mailService,
                              IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _mailService = mailService;
        }

        public async Task<bool> HasValidPassword(Guid id)
        {
            var oUser = await _unitOfWork.AppUser.GetByIdAsync(id);

            if (oUser == null)
                return true;

            if (_passwordService.Check(oUser.Password, oUser.Identification) ||
                _passwordService.Check(oUser.Password, oUser.IdentificationWithoutHypens))
                return false;

            return true;
        }

        public async Task<BusisnessResult> TryChangeAlias(Guid userId, string alias)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrWhiteSpace(alias))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                return oResult;
            }

            if (alias.Trim().Length > 10)
            {
                oResult.Message = "El alias no puede superar 10 caracteres.";

                return oResult;
            }

            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
            {
                oResult.Message = "Usuario no existe.";

                return oResult;
            }

            oUser.Alias = alias.Trim();

            _unitOfWork.AppUser.Update(oUser);

            oResult.Message = "Tu alias ha cambiado.";

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }

        public async Task<BusisnessResult> ChangePassword(Guid userId, string currentPass, string newPass, string newPassConf, bool isFirstLogin)
        {
            BusisnessResult oResult = new();

            if ((string.IsNullOrWhiteSpace(currentPass) && !isFirstLogin) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(newPassConf))
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

            if (!isFirstLogin)
            {
                if (!_passwordService.Check(oUser.Password, currentPass))
                {
                    oResult.Message = "La contraseña es incorrecta.";

                    return oResult;
                }
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

            if (newPass.Trim() == oUser.Identification || newPass.Trim() == oUser.IdentificationWithoutHypens)
            {
                oResult.Message = "La contraseña no puede ser igual a tu número de identificación.";

                return oResult;
            }

            string encPass = _passwordService.Hash(newPass.Trim());

            oUser.Password = encPass;

            oUser.ModifiedBy = oUser.Id.ToString();
            oUser.ModificationDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.AppUser.Update(oUser);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "Tu contraseña ha cambiado.";

            // El envío del correo se ejecuta en 1 hilo alterno
            Task.Run(() =>
            {
                _mailService.SendMail("Market Express", "Su contraseña ha cambiado.", oUser.Email);
            });

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

            if (oUsuarioDB.Status == EntityStatus.DESACTIVADO)
            {
                oResult.Message = "La cuenta está desactivada.";

                return oResult;
            }

            oUserRequest = oUsuarioDB;

            oResult.Success = true;

            return oResult;
        }

        public async Task<AppUser> GetUserInfo(Guid userId)
        {
            return await _unitOfWork.AppUser.GetByIdAsync(userId);
        }

        public async Task<List<Permission>> GetPermissionList(Guid userId)
        {
            return await _unitOfWork.AppUser.GetPermissionList(userId);
        }
    }
}
