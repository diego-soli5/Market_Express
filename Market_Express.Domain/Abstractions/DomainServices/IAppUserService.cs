using Market_Express.Domain.Entities;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IAppUserService
    {
        IEnumerable<AppUser> GetAll();
    }
}
