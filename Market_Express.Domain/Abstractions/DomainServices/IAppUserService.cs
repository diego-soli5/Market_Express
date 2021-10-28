using Market_Express.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAppUserService
    {
        IEnumerable<AppUser> GetAll();
        Task<BusisnessResult> ChangeStatus(Guid userToChangeId, Guid currentUserId);
        Task<BusisnessResult> Create(AppUser appUser, List<Guid> roles, Guid currentUserId);
    }
}
