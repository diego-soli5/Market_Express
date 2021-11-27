using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Market_Express.Domain.QueryFilter.AppUser;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market_Express.CrossCutting.CustomExceptions;
using Microsoft.Extensions.Options;
using Market_Express.CrossCutting.Options;

namespace Market_Express.Domain.Services
{
    public class AppUserService : BaseService, IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public AppUserService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService,
                              IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public IEnumerable<AppUser> GetAll(AppUserIndexQueryFilter filters)
        {
            var lstAppUser = _unitOfWork.AppUser.GetAll();

            if (filters.Name != null)
                lstAppUser = lstAppUser.Where(u => u.Name.Trim().ToUpper().Contains(filters.Name.Trim().ToUpper()));

            if (filters.Identification != null)
                lstAppUser = lstAppUser.Where(u => u.Identification.Trim().ToUpper().Contains(filters.Identification.Trim().ToUpper()) ||
                                                   u.IdentificationWithoutHypens.Trim().ToUpper().Contains(filters.Identification.Trim().ToUpper()));

            if (filters.Type != null)
                lstAppUser = lstAppUser.Where(u => u.Type == filters.Type);

            if (filters.Status != null)
                lstAppUser = lstAppUser.Where(u => u.Status == filters.Status);

            lstAppUser = lstAppUser.OrderBy(u => u.Name);

            return lstAppUser;
        }

        public List<string> SearchNames(string query, bool onlyAdmin = false)
        {
            var lstAppUser = _unitOfWork.AppUser.GetAll();

            if (query != null)
                lstAppUser = lstAppUser.Where(a => a.Name.ToUpper().Contains(query.ToUpper()));

            if (onlyAdmin)
                lstAppUser = lstAppUser.Where(a => a.Type == AppUserType.ADMINISTRADOR);

            return lstAppUser.OrderBy(a => a.Name)
                             .Select(a => a.Name)
                             .ToList();
        }

        public async Task<AppUser> GetById(Guid id, bool includeClient = false)
        {
            var oAppUser = await _unitOfWork.AppUser.GetByIdAsync(id, includeClient
                                                                      ? nameof(AppUser.Client)
                                                                      : null);

            if (oAppUser == null)
                throw new NotFoundException(id, nameof(AppUser));

            return oAppUser;
        }

        public async Task<BusisnessResult> Create(AppUser appUser, List<Guid> roles, Guid currentUserId)
        {
            BusisnessResult oResult = new();
            Client client;
            AppUser oUserToValidate;

            if (!ValidateAppUser(oResult, appUser, roles))
                return oResult;

            oUserToValidate = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Identification.Trim().ToUpper() == appUser.Identification.Trim().ToUpper());

            if (oUserToValidate != null)
            {
                oResult.Message = "El número de identificación ya está en uso.";
                oResult.ResultCode = 0;

                return oResult;
            }

            oUserToValidate = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Email.Trim().ToUpper() == appUser.Email.Trim().ToUpper());

            if (oUserToValidate != null)
            {
                oResult.Message = "El correo electrónico ya está en uso.";
                oResult.ResultCode = 1;

                return oResult;
            }

            oUserToValidate = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Phone.Trim().ToUpper() == appUser.Phone.Trim().ToUpper());

            if (oUserToValidate != null)
            {
                oResult.Message = "El número de teléfono ya está en uso.";
                oResult.ResultCode = 2;

                return oResult;
            }

            if (appUser.Type == AppUserType.ADMINISTRADOR)
            {
                appUser.AppUserRoles = new List<AppUserRole>();

                roles.ForEach(roleId =>
                {
                    appUser.AppUserRoles.Add(new AppUserRole
                    {
                        RoleId = roleId,
                        AppUserId = appUser.Id
                    });
                });
            }

            string sIdentificationWithoutHypens = appUser.IdentificationWithoutHypens;
            string sEncryptedPassword = _passwordService.Hash(sIdentificationWithoutHypens);

            appUser.Id = Guid.NewGuid();
            appUser.AddedBy = currentUserId.ToString();
            appUser.CreationDate = DateTimeUtility.NowCostaRica;
            appUser.Status = EntityStatus.ACTIVADO;
            appUser.Password = sEncryptedPassword;

            client = new();
            client.AppUserId = appUser.Id;
            client.AutoSync = false;

            _unitOfWork.AppUser.Create(appUser);
            _unitOfWork.Client.Create(client);

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            _unitOfWork.Cart.Create(new Cart
            {
                Id = new Guid(),
                ClientId = client.Id,
                OpeningDate = DateTimeUtility.NowCostaRica,
                Status = CartStatus.ABIERTO
            });

            await _unitOfWork.Save();

            oResult.Message = "El usuario se creó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> Edit(AppUser appUser, List<Guid> roles, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            var oUserFromDb = await _unitOfWork.AppUser.GetByIdAsync(appUser.Id, nameof(AppUser.Client));

            if (oUserFromDb == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            if (oUserFromDb.Type == AppUserType.ADMINISTRADOR)
            {
                if (roles?.Count <= 0)
                {
                    oResult.Message = "Se debe seleccionar al menos un rol.";

                    return oResult;
                }

                var oUserRolesInDb = _unitOfWork.AppUserRole.GetAllByUserId(appUser.Id);

                _unitOfWork.AppUserRole.Delete(oUserRolesInDb.ToList());

                roles.ForEach(id =>
                {
                    _unitOfWork.AppUserRole.Create(new AppUserRole
                    {
                        AppUserId = oUserFromDb.Id,
                        RoleId = id
                    });
                });
            }

            if (appUser.Status != oUserFromDb.Status)
            {
                if (appUser.Status == EntityStatus.DESACTIVADO && appUser.Id == currentUserId)
                {
                    oResult.Message = "No puedes desactivar tu cuenta de usuario, inicia sesión con otra cuenta.";

                    return oResult;
                }

                oUserFromDb.Status = appUser.Status;
            }

            if (appUser.Client.ClientCode != null)
            {
                oUserFromDb.Client.AutoSync = appUser.Client.AutoSync;

                _unitOfWork.Client.Update(oUserFromDb.Client);
            }

            oUserFromDb.ModifiedBy = currentUserId.ToString();
            oUserFromDb.ModificationDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.AppUser.Update(oUserFromDb);

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            oResult.Message = "El usuario se modificó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid userToChangeId, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if (userToChangeId == currentUserId)
            {
                oResult.Message = "No puedes desactivar tu cuenta de usuario, inicia sesión con otra cuenta.";

                return oResult;
            }

            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userToChangeId);

            if (oUser == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            if (oUser.Status == EntityStatus.ACTIVADO)
            {
                oUser.Status = EntityStatus.DESACTIVADO;

                oResult.ResultCode = 0;

                oResult.Message = "La cuenta de usuario se ha desactivado.";
            }
            else
            {
                oUser.Status = EntityStatus.ACTIVADO;

                oResult.ResultCode = 1;

                oResult.Message = "La cuenta de usuario se ha activado.";
            }

            oUser.ModificationDate = DateTimeUtility.NowCostaRica;
            oUser.ModifiedBy = currentUserId.ToString();

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }

        #region UTILITY METHODS
        private bool ValidateAppUser(BusisnessResult result, AppUser appUser, List<Guid> roles)
        {
            if (string.IsNullOrEmpty(appUser.Name))
            {
                result.Message = "El campo nombre es obligatorio.";

                return false;
            }
            else if (appUser.Name.Length > 50)
            {
                result.Message = "El campo nombre no puede superar los 50 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(appUser.Identification))
            {
                result.Message = "El campo identificación es obligatorio.";

                return false;
            }
            else if (appUser.Identification.Length > 12)
            {
                result.Message = "El campo identificación no puede superar los 12 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(appUser.Email))
            {
                result.Message = "El campo correo electrónico es obligatorio.";

                return false;
            }
            else if (appUser.Email.Length > 40)
            {
                result.Message = "El campo correo electrónico no puede superar los 40 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(appUser.Phone))
            {
                result.Message = "El campo teléfono es obligatorio.";

                return false;
            }
            else if (appUser.Phone.Length < 8 || appUser.Phone.Length > 8)
            {
                result.Message = "El campo teléfono debe contener 8 caracteres.";

                return false;
            }

            if (appUser.Type == AppUserType.ADMINISTRADOR && roles?.Count <= 0)
            {
                result.Message = "Se debe seleccionar al menos un rol.";

                return false;
            }

            return true;
        }
        #endregion
    }
}
