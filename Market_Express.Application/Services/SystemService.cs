using Market_Express.Application.DTOs.System;
using Market_Express.Domain.Abstractions.Repositories;
using Market_Express.Domain.EntityConstants;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market_Express.Application.Services
{
    public class SystemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SyncArticulos(List<ArticuloSyncDTO> articulosPOS)
        {
            if (articulosPOS?.Count < 0)
                return false;

            List<InventarioArticulo> articulosNuevosParaAgregar = new List<InventarioArticulo>();
            bool esNuevo = false;
            int articulosActualizados = 0;
            int articulosAgregados = 0;


            var articulosBD = _unitOfWork.Articulo.GetAll();

            articulosPOS.ForEach(articuloPOS =>
            {
                esNuevo = true;

                articulosBD.ToList().ForEach(articuloBD =>
                {
                    if(articuloBD.Id == articuloPOS.Id)
                    {
                        if (articuloBD.AutoSinc)
                        {
                            articuloBD.Descripcion = articuloPOS.Descripcion;
                            articuloBD.CodigoBarras = articuloPOS.CodigoBarras;
                            articuloBD.Precio = articuloPOS.Precio;

                            _unitOfWork.Articulo.Update(articuloBD);

                            articulosActualizados++;
                        }

                        esNuevo = false;
                    }
                });

                if (esNuevo)
                {
                    articulosNuevosParaAgregar.Add(new InventarioArticulo
                    {
                        Id = articuloPOS.Id,
                        Descripcion = articuloPOS.Descripcion,
                        CodigoBarras = articuloPOS.CodigoBarras,
                        Precio = articuloPOS.Precio,
                        Estado = EstadoArticulo.ACTIVADO
                    });
                }

            });

            articulosAgregados = articulosNuevosParaAgregar.Count;

            if (articulosAgregados > 0)
                _unitOfWork.Articulo.Create(articulosNuevosParaAgregar);

            if (articulosActualizados > 0 || articulosAgregados > 0)
                await _unitOfWork.Save();
            
            return true;
        }
    }
}
