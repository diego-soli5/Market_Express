using System;
using System.Threading.Tasks;

namespace Market_Express.Domain.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IArticleRepository Article { get; }
        public IClientRepository Client { get; }
        public IAppUserRepository AppUser { get; }
        public ICartRepository Cart { get; }

        Task BeginTransactionAsync();
        Task RollBackAsync();
        Task CommitTransactionAsync();
        Task<bool> Save();
    }
}
