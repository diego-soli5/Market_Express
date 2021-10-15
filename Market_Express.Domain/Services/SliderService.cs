using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class SliderService : BaseService, ISliderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobStorageService _storageService;

        public SliderService(IUnitOfWork unitOfWork,
                             IAzureBlobStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public IEnumerable<Slider> GetAll()
        {
            return _unitOfWork.Slider.GetAll();
        }

        public async Task<Slider> GetById(Guid sliderId)
        {
            var oSlider = await _unitOfWork.Slider.GetByIdAsync(sliderId);

            if (oSlider == null)
                throw new NotFoundException(sliderId, nameof(Slider));

            return oSlider;
        }

        public async Task<BusisnessResult> Create(string name, IFormFile image, Guid userId)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrWhiteSpace(name?.Trim()))
            {
                oResult.Message = "El campo es obligatirio.";

                oResult.ResultCode = 1;

                return oResult;
            }

            if(image == null)
            {
                oResult.Message = "El campo es obligatorio.";

                oResult.ResultCode = 2;

                return oResult;
            }

            if (!IsValidImage(image))
            {
                oResult.Message = "El formato de imagen es invalido.";

                oResult.ResultCode = 2;

                return oResult;
            }

            string sImageName = await _storageService.CreateBlobAsync(image);

            Slider oSlider = new()
            {
                Name = name,
                Image = sImageName,
                Status = EntityStatus.ACTIVADO,
                CreationDate = DateTimeUtility.NowCostaRica,
                AddedBy = userId.ToString()
            };

            _unitOfWork.Slider.Create(oSlider);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "El slider se creó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> Update(Slider slider, IFormFile image, Guid userId)
        {
            BusisnessResult oResult = new();
            string sNewImageName;

            if (string.IsNullOrWhiteSpace(slider.Name?.Trim()))
            {
                oResult.Message = "El campo es obligatirio.";

                oResult.ResultCode = 1;

                return oResult;
            }

            if (image?.Length > 0)
            {
                if (!IsValidImage(image))
                {
                    oResult.Message = "El formato de imagen es invalido.";

                    oResult.ResultCode = 2;

                    return oResult;
                }
            }

            var oSliderFromDb = await _unitOfWork.Slider.GetByIdAsync(slider.Id);

            if (oSliderFromDb == null)
            {
                oResult.Message = "El slider no existe.";

                oResult.ResultCode = 3;

                return oResult;
            }

            if (image?.Length > 0)
            {
                sNewImageName = await _storageService.CreateBlobAsync(image);

                await _storageService.DeleteBlobAsync(oSliderFromDb.Image);

                oSliderFromDb.Image = sNewImageName;
            }

            oSliderFromDb.Name = slider.Name;
            oSliderFromDb.ModifiedBy = userId.ToString();
            oSliderFromDb.ModificationDate = DateTimeUtility.NowCostaRica;
            oSliderFromDb.Status = slider.Status;

            _unitOfWork.Slider.Update(oSliderFromDb);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "El slider se modificó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid sliderId, Guid userId)
        {
            BusisnessResult oResult = new();

            var oSlider = await _unitOfWork.Slider.GetByIdAsync(sliderId);
        
            if(oSlider == null)
            {
                oResult.Message = "El slider no existe.";

                return oResult;
            }

            if(oSlider.Status == EntityStatus.ACTIVADO)
            {
                oSlider.Status = EntityStatus.DESACTIVADO;

                oResult.ResultCode = 0; //Cambia estilo CSS boton (Danger)

                oResult.Message = "El slider se ha desactivado.";
            }
            else
            {
                oSlider.Status = EntityStatus.ACTIVADO;

                oResult.ResultCode = 1; //Cambia estilo CSS boton (Success)

                oResult.Message = "El slider se ha activado.";
            }

            oSlider.ModificationDate = DateTimeUtility.NowCostaRica;

            oSlider.ModifiedBy = userId.ToString();

            _unitOfWork.Slider.Update(oSlider);

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }
    }
}
