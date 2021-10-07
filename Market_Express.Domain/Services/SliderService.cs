using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.Entities;
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

        public async Task<BusisnessResult> Create(string name, IFormFile image)
        {
            BusisnessResult oResult = new();

            if (string.IsNullOrWhiteSpace(name.Trim()))
            {
                oResult.Message = "El campo es obligatirio.";

                oResult.Message_Code = 1;
            }

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
