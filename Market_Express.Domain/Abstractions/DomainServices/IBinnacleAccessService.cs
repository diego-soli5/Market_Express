using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IBinnacleAccessService
    {
        Task RegisterAccess(Guid userId);
        Task RegisterExit(Guid userId);
    }
}
