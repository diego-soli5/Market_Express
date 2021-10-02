using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAccountService
    {
        BusisnessResult TryAuthenticate(ref AppUser usuarioRequest);
        Task<List<Permission>> GetPermissionList(Guid id);
        Task<AppUser> GetUserInfo(Guid id);
    }
}
