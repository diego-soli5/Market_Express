using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<List<Permiso>> GetPermisosAsync(Guid id);
    }
}
