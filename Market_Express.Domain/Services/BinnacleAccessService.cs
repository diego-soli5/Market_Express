using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class BinnacleAccessService : IBinnacleAccessService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BinnacleAccessService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterAccess(Guid userId)
        {
            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
                return;

            BinnacleAccess oBinnacleAccess = new()
            {
                Id = new Guid(),
                AppUserId = userId,
                EntryDate = DateTimeUtility.NowCostaRica,
                ExitDate = null
            };

            _unitOfWork.BinnacleAccess.Create(oBinnacleAccess);

            await _unitOfWork.Save();
        }

        public async Task RegisterExit(Guid userId)
        {
            var oUser = await _unitOfWork.AppUser.GetByIdAsync(userId);

            if (oUser == null)
                return;

            var oBinnacleAccess = await _unitOfWork.BinnacleAccess.GetLastAccessByUserId(userId);

            if (oBinnacleAccess == null)
                return;

            oBinnacleAccess.ExitDate = DateTimeUtility.NowCostaRica;

            _unitOfWork.BinnacleAccess.Update(oBinnacleAccess);

            await _unitOfWork.Save();
        }
    }
}
