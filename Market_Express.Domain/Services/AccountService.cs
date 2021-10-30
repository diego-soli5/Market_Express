using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BusisnessResult> EditAddress(Address address)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrEmpty(address.Name) || string.IsNullOrEmpty(address.Detail))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                return oResult;
            }

            var addressFromDb = await _unitOfWork.Address.GetByIdAsync(address.Id);

            if (addressFromDb == null)
            {
                oResult.Message = "La dirección no existe.";

                return oResult;
            }

            addressFromDb.Name = address.Name;
            addressFromDb.Detail = address.Detail;

            _unitOfWork.Address.Update(addressFromDb);

            oResult.Success = await _unitOfWork.Save();
            oResult.Message = "La dirección se modificó exitosamente.";

            return oResult;
        }

        public async Task<BusisnessResult> CreateAddress(Guid userId, Address address)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrEmpty(address.Name) || string.IsNullOrEmpty(address.Detail))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                return oResult;
            }

            var oClient = _unitOfWork.Client.GetFirstOrDefault(x => x.AppUserId == userId);

            if (oClient == null)
            {
                oResult.Message = "Usuario no existe.";

                return oResult;
            }

            var lstUserAddresses = (await _unitOfWork.Address.GetAllByUserId(userId)).ToList();

            if (lstUserAddresses?.Count >= 3)
            {
                oResult.Message = "No se puede crear la dirección, el maximo permitido son 3.";

                return oResult;
            }

            address.ClientId = oClient.Id;

            _unitOfWork.Address.Create(address);

            oResult.Success = await _unitOfWork.Save();
            oResult.Message = "La dirección se agregó exitosamente.";

            return oResult;
        }

        public async Task<Address> GetAddressInfo(Guid addressId)
        {
            return await _unitOfWork.Address.GetByIdAsync(addressId);
        }

        public async Task<AppUser> GetUserInfo(Guid userId)
        {
            return await _unitOfWork.AppUser.GetByIdAsync(userId);
        }

        public async Task<IEnumerable<Address>> GetAddressList(Guid userId)
        {
            return await _unitOfWork.Address.GetAllByUserId(userId);
        }

        public async Task<List<Permission>> GetPermissionList(Guid userId)
        {
            return await _unitOfWork.AppUser.GetPermissionList(userId);
        }
    }
}
