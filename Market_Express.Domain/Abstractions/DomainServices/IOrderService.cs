using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.DomainServices
{
    public interface IOrderService
    {
        Task<BusisnessResult> Generate(Guid userId);
    }
}
