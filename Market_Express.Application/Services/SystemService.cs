using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.EntityConstants;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Market_Express.CrossCutting.Response;
using Market_Express.Domain.Abstractions.ApplicationServices;
using Market_Express.Domain.Abstractions.Validations;

namespace Market_Express.Application.Services
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsuarioValidations _usuarioValidations;
        private readonly IClienteValidations _clienteValidations;
        private readonly IArticuloValidations _articuloValidations;

        public SystemService(IUnitOfWork unitOfWork,
                             IUsuarioValidations usuarioValidations,
                             IClienteValidations clienteValidations,
                             IArticuloValidations articuloValidations)
        {
            _unitOfWork = unitOfWork;
            _usuarioValidations = usuarioValidations;
            _clienteValidations = clienteValidations;
            _articuloValidations = articuloValidations;
        }


        public async Task<SyncResponse> SyncClients(List<Cliente> clientsPOS)
        {
            SyncResponse response = new();

            if (clientsPOS?.Count <= 0)
                return response;

            List<Cliente> clientsToAdd = new();
            bool isNew = false;
            int added = 0;
            int updated = 0;

            var clientsDB = _unitOfWork.Cliente.GetAll(nameof(Cliente.Usuario));

            clientsPOS.ForEach(clientPOS =>
            {
                isNew = true;

                foreach (var clientDB in clientsDB)
                {
                    if (clientDB.Id == clientPOS.Id)
                    {
                        if (clientDB.AutoSinc)
                        {
                            if (clientDB.Usuario.Nombre.Trim() != clientPOS.Usuario.Nombre?.Trim() ||
                                clientDB.Usuario.Cedula.Trim() != clientPOS.Usuario.Cedula?.Trim() ||
                                clientDB.Usuario.Email.Trim() != clientPOS.Usuario.Email?.Trim() ||
                                clientDB.Usuario.Telefono.Trim() != clientPOS.Usuario.Telefono?.Trim())
                            {
                                _usuarioValidations.Usuario = clientPOS.Usuario;

                                if (!_usuarioValidations.ExistsEmail())
                                    clientDB.Usuario.Email ??= clientPOS.Usuario.Email?.Trim();


                                if (!_usuarioValidations.ExistsCedula())
                                    clientDB.Usuario.Cedula ??= clientPOS.Usuario.Cedula?.Trim();


                                clientDB.Usuario.Nombre ??= clientPOS.Usuario.Nombre?.Trim();
                                clientDB.Usuario.Telefono ??= clientPOS.Usuario.Telefono?.Trim();

                                _unitOfWork.Usuario.Update(clientDB.Usuario);

                                updated++;
                            }
                        }

                        isNew = false;

                        break;
                    }
                }

                if (isNew)
                {
                    if (!clientsToAdd.Contains(clientPOS))
                    {
                        _clienteValidations.Cliente = clientPOS;
                        _usuarioValidations.Usuario = clientPOS.Usuario;

                        if (!_clienteValidations.ExistsCodCliente() &&
                            !_usuarioValidations.ExistsCedula() &&
                            !_usuarioValidations.ExistsEmail())
                        {

                            clientPOS.Usuario.Estado = EstadoUsuario.ACTIVADO;
                            clientsToAdd.Add(clientPOS);
                        }
                    }
                }
            });

            added = clientsToAdd.Count;

            if (added > 0)
                _unitOfWork.Cliente.Create(clientsToAdd);

            if (added > 0 || updated > 0)
                await _unitOfWork.Save();

            response.AddedCount = added;
            response.UpdatedCount = updated;

            return response;
        }

        public async Task<SyncResponse> SyncArticles(List<InventarioArticulo> articlesPOS)
        {
            SyncResponse response = new();

            if (articlesPOS?.Count <= 0)
                return response;

            List<InventarioArticulo> articlesToAdd = new();
            bool isNew = false;
            int added = 0;
            int updated = 0;


            var articlesDB = _unitOfWork.Articulo.GetAll();

            articlesPOS.ForEach(articlePOS =>
            {
                isNew = true;

                foreach (var articleDB in articlesDB)
                {
                    if (articleDB.Id == articlePOS.Id)
                    {
                        if (articleDB.AutoSinc)
                        {
                            if (articleDB.Descripcion.Trim() != articlePOS.Descripcion?.Trim() ||
                                articleDB.CodigoBarras.Trim() != articlePOS.CodigoBarras?.Trim() ||
                                articleDB.Precio != articlePOS.Precio)
                            {
                                _articuloValidations.Articulo = articlePOS;

                                if (!_articuloValidations.ExistsCodigoBarras())
                                    articleDB.CodigoBarras ??= articlePOS.CodigoBarras.Trim();

                                articleDB.Descripcion ??= articlePOS.Descripcion.Trim();
                                articleDB.Precio = articlePOS.Precio;

                                _unitOfWork.Articulo.Update(articleDB);

                                updated++;
                            }
                        }

                        isNew = false;

                        break;
                    }
                }

                if (isNew)
                {
                    if (!articlesToAdd.Contains(articlePOS))
                    {
                        _articuloValidations.Articulo = articlePOS;

                        if (!_articuloValidations.ExistsCodigoBarras())
                        {
                            articlePOS.Estado = EstadoArticulo.ACTIVADO;
                            articlesToAdd.Add(articlePOS);
                        }
                    }
                }
            });

            added = articlesToAdd.Count;

            if (added > 0)
                _unitOfWork.Articulo.Create(articlesToAdd);

            if (added > 0 || updated > 0)
                await _unitOfWork.Save();

            response.AddedCount = added;
            response.UpdatedCount = updated;

            return response;
        }
    }
}
