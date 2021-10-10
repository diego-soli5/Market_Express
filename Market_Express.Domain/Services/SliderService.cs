using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
using Market_Express.Domain.EntityConstants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class SliderService : ISliderService
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

        public async Task<Slider> GetById(Guid id)
        {
            return await _unitOfWork.Slider.GetByIdAsync(id);
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
                Status = SliderConstants.ACTIVADO,
                CreationDate = DateTimeUtility.NowCostaRica,
                AddedBy = userId.ToString()
            };

            _unitOfWork.Slider.Create(oSlider);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "El slider se creó exitosamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid sliderId, Guid userId)
        {
            BusisnessResult oResult = new();

            var oSlider = await _unitOfWork.Slider.GetByIdAsync(sliderId);
        
            if(oSlider == null)
            {
                oResult.Message = "Slider no existe.";

                return oResult;
            }

            if(oSlider.Status == SliderConstants.ACTIVADO)
            {
                oSlider.Status = SliderConstants.DESACTIVADO;

                oResult.ResultCode = 0; //Cambia estilo CSS boton (Danger)

                oResult.Message = "El slider se ha desactivado.";
            }
            else
            {
                oSlider.Status = SliderConstants.ACTIVADO;

                oResult.ResultCode = 1; //Cambia estilo CSS boton (Success)

                oResult.Message = "El slider se ha activado.";
            }

            oSlider.ModificationDate = DateTimeUtility.NowCostaRica;

            oSlider.ModifiedBy = userId.ToString();

            _unitOfWork.Slider.Update(oSlider);

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }

        #region VALIDATION METHODS
        private bool IsValidImage(IFormFile image)
        {
            var validImageTypes = new[] {

                    "image/png",
                    "image/jpg",
                    "image/jpeg"
                };

            if (!validImageTypes.Any(x => x == image.ContentType))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
