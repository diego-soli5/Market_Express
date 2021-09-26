using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.EntityConstants;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market_Express.CrossCutting.Response;
using Market_Express.Domain.Abstractions.ApplicationServices;
using Market_Express.Domain.Abstractions.Validations;
using Market_Express.Domain.Abstractions.InfrastructureServices;
using System;

namespace Market_Express.Application.Services
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppUserValidations _usuarioValidations;
        private readonly IClientValidations _clienteValidations;
        private readonly IArticleValidations _articuloValidations;
        private readonly IPasswordService _passwordService;

        public SystemService(IUnitOfWork unitOfWork,
                             IAppUserValidations usuarioValidations,
                             IClientValidations clienteValidations,
                             IArticleValidations articuloValidations,
                             IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _usuarioValidations = usuarioValidations;
            _clienteValidations = clienteValidations;
            _articuloValidations = articuloValidations;
            _passwordService = passwordService;
        }


        public async Task<SyncResponse> SyncClients(List<Client> lstClientsToClient)
        {
            SyncResponse oResponse = new();

            if (lstClientsToClient?.Count <= 0)
                return oResponse;

            List<Client> lstClientsToAdd = new();
            bool bIsNew = false;
            int iAdded = 0;
            int iUpdated = 0;

            var lstClientsFromDb = _unitOfWork.Client.GetAll(nameof(Client.AppUser));

            lstClientsToClient.ForEach(oClientPOS =>
            {
                bIsNew = true;

                foreach (var oClientDb in lstClientsFromDb)
                {
                    if (oClientDb.Id == oClientPOS.Id)
                    {
                        if (oClientDb.AutoSync)
                        {
                            if (oClientDb.AppUser.Name.Trim() != oClientPOS.AppUser.Name?.Trim() ||
                                oClientDb.AppUser.Identification.Trim() != oClientPOS.AppUser.Identification?.Trim() ||
                                oClientDb.AppUser.Email.Trim() != oClientPOS.AppUser.Email?.Trim() ||
                                oClientDb.AppUser.Phone.Trim() != oClientPOS.AppUser.Phone?.Trim())
                            {
                                _usuarioValidations.Usuario = oClientPOS.AppUser;

                                if (!_usuarioValidations.ExistsEmail())
                                    oClientDb.AppUser.Email ??= oClientPOS.AppUser.Email?.Trim();


                                if (!_usuarioValidations.ExistsCedula())
                                    oClientDb.AppUser.Identification ??= oClientPOS.AppUser.Identification?.Trim();


                                oClientDb.AppUser.Name ??= oClientPOS.AppUser.Name?.Trim();
                                oClientDb.AppUser.Phone ??= oClientPOS.AppUser.Phone?.Trim();

                                oClientDb.AppUser.ModifiedBy = SystemConstants.SYSTEM;

                                _unitOfWork.AppUser.Update(oClientDb.AppUser);

                                iUpdated++;
                            }
                        }

                        bIsNew = false;

                        break;
                    }
                }

                if (bIsNew)
                {
                    if (!lstClientsToAdd.Contains(oClientPOS))
                    {
                        _clienteValidations.Cliente = oClientPOS;
                        _usuarioValidations.Usuario = oClientPOS.AppUser;

                        if (!_clienteValidations.ExistsCodCliente() &&
                            !_usuarioValidations.ExistsCedula() &&
                            !_usuarioValidations.ExistsEmail())
                        {
                            oClientPOS.AppUser.CreationDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                                                            TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                            oClientPOS.AppUser.Status = AppUserConstants.ACTIVADO;
                            oClientPOS.AppUser.AddedBy = SystemConstants.SYSTEM;
                            oClientPOS.AppUser.Password = _passwordService.Hash(oClientPOS.AppUser.IdentificationWithoutHypens);

                            lstClientsToAdd.Add(oClientPOS);
                        }
                    }
                }
            });

            iAdded = lstClientsToAdd.Count;

            if (iAdded > 0)
                _unitOfWork.Client.Create(lstClientsToAdd);

            if (iAdded > 0 || iUpdated > 0)
                await _unitOfWork.Save();

            oResponse.AddedCount = iAdded;
            oResponse.UpdatedCount = iUpdated;

            return oResponse;
        }

        public async Task<SyncResponse> SyncArticles(List<Article> lstArticlesToSync)
        {
            SyncResponse oResponse = new();

            if (lstArticlesToSync?.Count <= 0)
                return oResponse;

            List<Article> lstArticlesToAdd = new();
            bool bIsNew = false;
            int iAdded = 0;
            int iUpdated = 0;


            var lstArticlesFromDb = _unitOfWork.Article.GetAll();

            lstArticlesToSync.ForEach(oArticlePOS =>
            {
                bIsNew = true;

                foreach (var oArticleDb in lstArticlesFromDb)
                {
                    if (oArticleDb.Id == oArticlePOS.Id)
                    {
                        if (oArticleDb.AutoSync)
                        {
                            if (oArticleDb.Description.Trim() != oArticlePOS.Description?.Trim() ||
                                oArticleDb.BarCode.Trim() != oArticlePOS.BarCode?.Trim() ||
                                oArticleDb.Price != oArticlePOS.Price)
                            {
                                _articuloValidations.Articulo = oArticlePOS;

                                if (!_articuloValidations.ExistsCodigoBarras())
                                    oArticleDb.BarCode ??= oArticlePOS.BarCode.Trim();

                                oArticleDb.Description ??= oArticlePOS.Description.Trim();
                                oArticleDb.Price = oArticlePOS.Price;
                                oArticleDb.ModifiedBy = SystemConstants.SYSTEM;

                                _unitOfWork.Article.Update(oArticleDb);

                                iUpdated++;
                            }
                        }

                        bIsNew = false;

                        break;
                    }
                }

                if (bIsNew)
                {
                    if (!lstArticlesToAdd.Contains(oArticlePOS))
                    {
                        _articuloValidations.Articulo = oArticlePOS;

                        if (!_articuloValidations.ExistsCodigoBarras())
                        {
                            oArticlePOS.CreationDate = TimeZoneInfo.ConvertTime(DateTime.Now,
                                                            TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                            oArticlePOS.Status = ArticleConstants.ACTIVADO;
                            oArticlePOS.AddedBy = SystemConstants.SYSTEM;

                            lstArticlesToAdd.Add(oArticlePOS);
                        }
                    }
                }
            });

            iAdded = lstArticlesToAdd.Count;

            if (iAdded > 0)
                _unitOfWork.Article.Create(lstArticlesToAdd);

            if (iAdded > 0 || iUpdated > 0)
                await _unitOfWork.Save();

            oResponse.AddedCount = iAdded;
            oResponse.UpdatedCount = iUpdated;

            return oResponse;
        }
    }
}
