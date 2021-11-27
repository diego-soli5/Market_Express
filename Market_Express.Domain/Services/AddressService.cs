using Market_Express.CrossCutting.Options;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class AddressService : BaseService, IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork,
                              IOptions<PaginationOptions> paginationOptions)
            : base(paginationOptions)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BusisnessResult> CreateFromCart(Address address, Guid userId)
        {
            int iCount = (await _unitOfWork.Address.GetAllByUserId(userId)).Count();

            address.Name = $"Dirección {iCount + 1}";

            return await Create(address,userId);
        }

        public async Task<BusisnessResult> Create(Address address, Guid userId)
        {
            BusisnessResult oResult = new();

            if (!ValidateAddress(oResult, address))
                return oResult;

            var oClient = _unitOfWork.Client.GetFirstOrDefault(x => x.AppUserId == userId);

            if (oClient == null)
            {
                oResult.Message = "El usuario no existe.";

                return oResult;
            }

            var lstUserAddresses = (await _unitOfWork.Address.GetAllByUserId(userId)).ToList();

            if (lstUserAddresses?.Count >= 3)
            {
                oResult.Message = "No se puede crear la dirección, el maximo permitido son 3.";

                return oResult;
            }

            address.ClientId = oClient.Id;
            address.InUse = lstUserAddresses.Count <= 0;

            _unitOfWork.Address.Create(address);

            oResult.Success = await _unitOfWork.Save();
            oResult.Message = "La dirección se agregó exitosamente.";

            return oResult;
        }

        public async Task<BusisnessResult> Edit(Address address, Guid userId)
        {
            BusisnessResult oResult = new();

            if (!ValidateAddress(oResult, address))
                return oResult;

            var addressFromDb = await _unitOfWork.Address.GetByIdAsync(address.Id);

            if (addressFromDb == null)
            {
                oResult.Message = "La dirección no existe.";

                return oResult;
            }

            if (address.InUse)
            {
                var lstUserAddress = (await _unitOfWork.Address.GetAllByUserId(userId)).ToList();

                lstUserAddress.ForEach(addressToUpdate =>
                {
                    if (addressFromDb.Id != addressToUpdate.Id)
                    {
                        addressToUpdate.InUse = false;

                        _unitOfWork.Address.Update(addressToUpdate);
                    }
                });
            }

            addressFromDb.Name = address.Name;
            addressFromDb.Detail = address.Detail;

            _unitOfWork.Address.Update(addressFromDb);

            oResult.Success = await _unitOfWork.Save();
            oResult.Message = "La dirección se modificó exitosamente.";

            return oResult;
        }

        public async Task<BusisnessResult> SetForUse(Guid addressId, Guid userId)
        {
            BusisnessResult oResult = new();

            var addressFromDb = await _unitOfWork.Address.GetByIdAsync(addressId);

            if (addressFromDb == null)
            {
                oResult.Message = "La dirección no existe.";

                return oResult;
            }

            if (addressFromDb.InUse)
            {
                oResult.Success = true;

                oResult.Message = "La dirección ya está establecida.";

                return oResult;
            }

            var lstUserAddress = await _unitOfWork.Address.GetAllByUserId(userId);

            lstUserAddress.ToList().ForEach(addressToUpdate =>
            {
                if(addressToUpdate.Id != addressFromDb.Id)
                {
                    addressToUpdate.InUse = false;

                    _unitOfWork.Address.Update(addressToUpdate);
                }
            });

            addressFromDb.InUse = true;

            _unitOfWork.Address.Update(addressFromDb);

            oResult.Success = await SaveWithTransaction(_unitOfWork);

            oResult.Message = "Se estableció la dirección para ser usada.";

            return oResult;
        }

        public async Task<Address> GetById(Guid addressId)
        {
            return await _unitOfWork.Address.GetByIdAsync(addressId);
        }

        public async Task<IEnumerable<Address>> GetAllByUserId(Guid userId)
        {
            return await _unitOfWork.Address.GetAllByUserId(userId);
        }

        #region UTILITY METHODS
        public bool ValidateAddress(BusisnessResult result, Address address)
        {
            if (string.IsNullOrEmpty(address.Name))
            {
                result.Message = "El campo nombre es obligatorio.";

                return false;
            }
            else if (address.Name.Length > 50)
            {
                result.Message = "El campo nombre no puede superar los 50 caracteres.";

                return false;
            }

            if (string.IsNullOrEmpty(address.Detail))
            {
                result.Message = "El campo detalle es obligatorio.";

                return false;
            }
            else if (address.Name.Length > 255)
            {
                result.Message = "El campo detalle no puede superar los 255 caracteres.";

                return false;
            }

            return true;
        }
        #endregion
    }
}
