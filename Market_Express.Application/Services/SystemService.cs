using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.EntityConstants;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market_Express.CrossCutting.Response;
using Market_Express.Domain.Abstractions.ApplicationServices;

namespace Market_Express.Application.Services
{
    public class SystemService : ISystemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                clientsDB.ToList().ForEach(clientDB =>
                {
                    if (clientDB.Id == clientPOS.Id)
                    {
                        if (clientDB.AutoSinc)
                        {
                            if (clientDB.Usuario.Nombre != clientPOS.Usuario.Nombre ||
                               clientDB.Usuario.Cedula != clientPOS.Usuario.Cedula ||
                               clientDB.Usuario.Email != clientPOS.Usuario.Email ||
                               clientDB.Usuario.Telefono != clientPOS.Usuario.Telefono)
                            {
                                clientDB.Usuario.Nombre = clientPOS.Usuario.Nombre;
                                clientDB.Usuario.Cedula = clientPOS.Usuario.Cedula;
                                clientDB.Usuario.Email = clientPOS.Usuario.Email;
                                clientDB.Usuario.Telefono = clientPOS.Usuario.Telefono;

                                _unitOfWork.Usuario.Update(clientDB.Usuario);

                                updated++;
                            }
                        }

                        isNew = false;
                    }

                    if (isNew)
                    {
                        clientDB.Usuario.Estado = EstadoUsuario.ACTIVADO;
                        clientsToAdd.Add(clientDB);
                    }
                });
            });

            added = clientsToAdd.Count;

            if (added > 0)
                _unitOfWork.Cliente.Create(clientsToAdd);

            if (added > 0 || updated > 0)
                await _unitOfWork.Save();

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

            articlesPOS.ForEach(articuloPOS =>
            {
                isNew = true;

                articlesDB.ToList().ForEach(articuloBD =>
                {
                    if (articuloBD.Id == articuloPOS.Id)
                    {
                        if (articuloBD.AutoSinc)
                        {
                            if (articuloBD.Descripcion.Trim() != articuloPOS.Descripcion.Trim() ||
                               articuloBD.CodigoBarras.Trim() != articuloPOS.CodigoBarras.Trim() ||
                               articuloBD.Precio != articuloPOS.Precio)
                            {
                                articuloBD.Descripcion = articuloPOS.Descripcion.Trim();
                                articuloBD.CodigoBarras = articuloPOS.CodigoBarras.Trim();
                                articuloBD.Precio = articuloPOS.Precio;

                                _unitOfWork.Articulo.Update(articuloBD);

                                updated++;
                            }
                        }

                        isNew = false;
                    }
                });

                if (isNew)
                {
                    articuloPOS.Estado = EstadoArticulo.ACTIVADO;

                    articlesToAdd.Add(articuloPOS);
                }
            });

            added = articlesToAdd.Count;

             if (added > 0)
                _unitOfWork.Articulo.Create(articlesToAdd);

            if (added > 0 || updated > 0)
                await _unitOfWork.Save();

            return response;
        }
    }
}
