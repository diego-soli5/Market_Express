using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAccountService
    {
        BusisnessResult TryAuthenticate(Usuario usuarioRequest);
        Task<List<Permiso>> GetPermisos(Guid id);
    }
}
