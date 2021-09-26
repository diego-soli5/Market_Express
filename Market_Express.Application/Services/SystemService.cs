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
        private readonly IUsuarioValidations _usuarioValidations;
        private readonly IClienteValidations _clienteValidations;
        private readonly IArticuloValidations _articuloValidations;
        private readonly IPasswordService _passwordService;

        public SystemService(IUnitOfWork unitOfWork,
                             IUsuarioValidations usuarioValidations,
                             IClienteValidations clienteValidations,
                             IArticuloValidations articuloValidations,
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

            var lstClientsFromDb = _unitOfWork.Cliente.GetAll(nameof(Client.Usuario));

            lstClientsToClient.ForEach(oClientPOS =>
            {
                bIsNew = true;

                foreach (var oClientDb in lstClientsFromDb)
                {
                    if (oClientDb.Id == oClientPOS.Id)
                    {
                        if (oClientDb.AutoSinc)
                        {
                            if (oClientDb.Usuario.Nombre.Trim() != oClientPOS.Usuario.Nombre?.Trim() ||
                                oClientDb.Usuario.Cedula.Trim() != oClientPOS.Usuario.Cedula?.Trim() ||
                                oClientDb.Usuario.Email.Trim() != oClientPOS.Usuario.Email?.Trim() ||
                                oClientDb.Usuario.Telefono.Trim() != oClientPOS.Usuario.Telefono?.Trim())
                            {
                                _usuarioValidations.Usuario = oClientPOS.Usuario;

                                if (!_usuarioValidations.ExistsEmail())
                                    oClientDb.Usuario.Email ??= oClientPOS.Usuario.Email?.Trim();


                                if (!_usuarioValidations.ExistsCedula())
                                    oClientDb.Usuario.Cedula ??= oClientPOS.Usuario.Cedula?.Trim();


                                oClientDb.Usuario.Nombre ??= oClientPOS.Usuario.Nombre?.Trim();
                                oClientDb.Usuario.Telefono ??= oClientPOS.Usuario.Telefono?.Trim();

                                oClientDb.Usuario.ModificadoPor = SystemConstants.SYSTEM;

                                _unitOfWork.Usuario.Update(oClientDb.Usuario);

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
                        _usuarioValidations.Usuario = oClientPOS.Usuario;

                        if (!_clienteValidations.ExistsCodCliente() &&
                            !_usuarioValidations.ExistsCedula() &&
                            !_usuarioValidations.ExistsEmail())
                        {
                            oClientPOS.Usuario.FecCreacion = TimeZoneInfo.ConvertTime(DateTime.Now,
                                                            TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                            oClientPOS.Usuario.Estado = UsuarioConstants.ACTIVADO;
                            oClientPOS.Usuario.AdicionadoPor = SystemConstants.SYSTEM;
                            oClientPOS.Usuario.Clave = _passwordService.Hash(oClientPOS.Usuario.GetCedulaSinGuiones);

                            lstClientsToAdd.Add(oClientPOS);
                        }
                    }
                }
            });

            iAdded = lstClientsToAdd.Count;

            if (iAdded > 0)
                _unitOfWork.Cliente.Create(lstClientsToAdd);

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


            var lstArticlesFromDb = _unitOfWork.Articulo.GetAll();

            lstArticlesToSync.ForEach(oArticlePOS =>
            {
                bIsNew = true;

                foreach (var oArticleDb in lstArticlesFromDb)
                {
                    if (oArticleDb.Id == oArticlePOS.Id)
                    {
                        if (oArticleDb.AutoSinc)
                        {
                            if (oArticleDb.Descripcion.Trim() != oArticlePOS.Descripcion?.Trim() ||
                                oArticleDb.CodigoBarras.Trim() != oArticlePOS.CodigoBarras?.Trim() ||
                                oArticleDb.Precio != oArticlePOS.Precio)
                            {
                                _articuloValidations.Articulo = oArticlePOS;

                                if (!_articuloValidations.ExistsCodigoBarras())
                                    oArticleDb.CodigoBarras ??= oArticlePOS.CodigoBarras.Trim();

                                oArticleDb.Descripcion ??= oArticlePOS.Descripcion.Trim();
                                oArticleDb.Precio = oArticlePOS.Precio;
                                oArticleDb.ModificadoPor = SystemConstants.SYSTEM;

                                _unitOfWork.Articulo.Update(oArticleDb);

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
                            oArticlePOS.FecCreacion = TimeZoneInfo.ConvertTime(DateTime.Now,
                                                            TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
                            oArticlePOS.Estado = ArticuloConstants.ACTIVADO;
                            oArticlePOS.AdicionadoPor = SystemConstants.SYSTEM;

                            lstArticlesToAdd.Add(oArticlePOS);
                        }
                    }
                }
            });

            iAdded = lstArticlesToAdd.Count;

            if (iAdded > 0)
                _unitOfWork.Articulo.Create(lstArticlesToAdd);

            if (iAdded > 0 || iUpdated > 0)
                await _unitOfWork.Save();

            oResponse.AddedCount = iAdded;
            oResponse.UpdatedCount = iUpdated;

            return oResponse;
        }
    }
}
