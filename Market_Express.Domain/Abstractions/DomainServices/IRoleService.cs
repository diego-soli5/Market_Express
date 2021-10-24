using Market_Express.Domain.Entities;
using System.Collections.Generic;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IRoleService
    {
        List<Role> GetAll();
    }
}
