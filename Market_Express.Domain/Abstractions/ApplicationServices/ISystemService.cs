using Market_Express.CrossCutting.Response;
using Market_Express.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.ApplicationServices
{
    public interface ISystemService
    {
        Task<SyncResponse> SyncArticles(List<InventarioArticulo> articulosPOS);
    }
}