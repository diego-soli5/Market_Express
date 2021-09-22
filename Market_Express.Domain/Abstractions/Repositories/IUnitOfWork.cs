using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IInventarioArticuloRepository Articulo { get; }

        Task BeginTransactionAsync();
        Task RollBackAsync();
        Task CommitTransactionAsync();
        Task<bool> Save();
    }
}
