using Market_Express.CrossCutting.CustomExceptions;
using Market_Express.CrossCutting.Options;
using Market_Express.CrossCutting.Utility;
using Market_Express.Domain.Abstractions.DomainServices;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.CustomEntities.Category;
using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Domain.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureBlobStorageService _storageService;
        private readonly CategoryOptions _categoryOptions;

        public CategoryService(IUnitOfWork unitOfWork,
                               IAzureBlobStorageService storageService,
                               IOptions<CategoryOptions> categoryOptions)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
            _categoryOptions = categoryOptions.Value;
        }

        public IEnumerable<Category> GetAll()
        {
            return _unitOfWork.Category.GetAll();
        }

        public IEnumerable<Category> GetAllActive()
        {
            return _unitOfWork.Category.GetAllActive();
        }

        public async Task<List<CategoryForSearch>> GetAllAvailableForSearch()
        {
            return await _unitOfWork.Category.GetAllAvailableForSearch();
        }

        public async Task<Category> GetById(Guid categoryId)
        {
            var oCategory = await _unitOfWork.Category.GetByIdAsync(categoryId);

            if (oCategory == null)
                throw new NotFoundException(categoryId, nameof(Category));

            return oCategory;
        }

        public async Task<(int, int)> GetArticleDetails(Guid categoryId)
        {
            return await _unitOfWork.Category.GetArticleDetails(categoryId);
        }

        public async Task<List<Category>> GetMostPopular(int? take = null)
        {
            //take = take is null ? _categoryOptions.DefaultTakeForMostPopular : take;

            return await _unitOfWork.Category.GetMostPopular(take);
        }

        public async Task<BusisnessResult> Create(Category category, IFormFile image, Guid userId)
        {
            BusisnessResult oResult = new();
            string sImageName = null;

            if (string.IsNullOrEmpty(category.Name) || string.IsNullOrWhiteSpace(category.Description))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                oResult.ResultCode = 1;

                return oResult;
            }

            if (image != null)
            {
                if (!IsValidImage(image))
                {
                    oResult.Message = "El formato de imagen es invalido.";

                    oResult.ResultCode = 2;

                    return oResult;
                }

                sImageName = await _storageService.CreateBlobAsync(image);
            }

            category.Status = EntityStatus.ACTIVADO;
            category.Image = sImageName;

            category.CreationDate = DateTimeUtility.NowCostaRica;
            category.AddedBy = userId.ToString();

            _unitOfWork.Category.Create(category);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "La categoría se creó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> Edit(Category category, IFormFile image, Guid userId)
        {
            BusisnessResult oResult = new();
            string sNewImageName = null;

            if (string.IsNullOrEmpty(category.Name) || string.IsNullOrWhiteSpace(category.Description))
            {
                oResult.Message = "No se pueden enviar campos vacíos.";

                oResult.ResultCode = 1;

                return oResult;
            }

            if (image != null)
            {
                if (!IsValidImage(image))
                {
                    oResult.Message = "El formato de imagen es invalido.";

                    oResult.ResultCode = 2;

                    return oResult;
                }
            }

            var oCategoryFromDb = await _unitOfWork.Category.GetByIdAsync(category.Id,nameof(Category.Articles));

            if (oCategoryFromDb == null)
            {
                oResult.Message = "La categoría no existe.";

                oResult.ResultCode = 1;

                return oResult;
            }

            if(category.Status == EntityStatus.DESACTIVADO && oCategoryFromDb.Status == EntityStatus.ACTIVADO)
            {
                if (oCategoryFromDb.Articles?.Count > 0)
                {
                    if (oCategoryFromDb.Articles.Any(art => art.Status == EntityStatus.ACTIVADO))
                    {
                        oResult.Message = "La categoría no se puede desactivar, para desactivarla debe desactivar los articulos relacionados.";

                        return oResult;
                    }
                }
            }

            if (image != null)
            {
                if (oCategoryFromDb.Image != null)
                    await _storageService.DeleteBlobAsync(oCategoryFromDb.Image);

                sNewImageName = await _storageService.CreateBlobAsync(image);

                oCategoryFromDb.Image = sNewImageName;
            }

            oCategoryFromDb.Name = category.Name;
            oCategoryFromDb.Description = category.Description;
            oCategoryFromDb.Status = category.Status;
            oCategoryFromDb.ModificationDate = DateTimeUtility.NowCostaRica;
            oCategoryFromDb.ModifiedBy = userId.ToString();

            _unitOfWork.Category.Update(oCategoryFromDb);

            oResult.Success = await _unitOfWork.Save();

            oResult.Message = "La categoría se modificó correctamente!";

            return oResult;
        }

        public async Task<BusisnessResult> ChangeStatus(Guid categoryId, Guid userId)
        {
            BusisnessResult oResult = new();

            var oCategory = await _unitOfWork.Category.GetByIdAsync(categoryId, nameof(Category.Articles));

            if (oCategory == null)
            {
                oResult.Message = "La categoría no existe.";

                return oResult;
            }

            if (oCategory.Status == EntityStatus.ACTIVADO)
            {
                if (oCategory.Articles?.Count > 0)
                {
                    if (oCategory.Articles.Any(art => art.Status == EntityStatus.ACTIVADO))
                    {
                        oResult.Message = "La categoría no se puede desactivar, para desactivarla debe desactivar los articulos relacionados.";

                        return oResult;
                    }
                }

                oCategory.Status = EntityStatus.DESACTIVADO;

                oResult.ResultCode = 0; //Cambia estilo CSS boton (Danger)

                oResult.Message = "La categoría se ha desactivado.";
            }
            else
            {
                oCategory.Status = EntityStatus.ACTIVADO;

                oResult.ResultCode = 1; //Cambia estilo CSS boton (Success)

                oResult.Message = "La categoría se ha activado.";
            }

            oCategory.ModificationDate = DateTimeUtility.NowCostaRica;

            oCategory.ModifiedBy = userId.ToString();

            _unitOfWork.Category.Update(oCategory);

            oResult.Success = await _unitOfWork.Save();

            return oResult;
        }
    }
}
