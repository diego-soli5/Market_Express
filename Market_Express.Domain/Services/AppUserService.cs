using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<AppUser> GetAll()
        {
            return _unitOfWork.AppUser.GetAll();
        }

        public async Task<BusisnessResult> ChangeStatus(Guid userToChangeId, Guid currentUserId)
        {
            BusisnessResult oResult = new();

            if(userToChangeId == currentUserId)
            {
                oResult.Message = "No puedes desactivar tu cuenta de usuario, inicia sesión con otra cuenta.";

                return oResult;
            }

            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userToChangeId);

            if(oUser == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            if (oUser.Status == EntityStatus.ACTIVADO)
            {
                oUser.Status = EntityStatus.DESACTIVADO;

                oResult.ResultCode = 0;
            }
            else
            {
                oUser.Status = EntityStatus.ACTIVADO;

                oResult.ResultCode = 1;
            }

            oUser.ModificationDate = DateTimeUtility.NowCostaRica;
            oUser.ModifiedBy = currentUserId.ToString();

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }
    }
}
