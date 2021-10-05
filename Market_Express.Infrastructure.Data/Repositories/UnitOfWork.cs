using Market_Express.Domain.Abstractions.Repositories;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region ATTRIBUTES
        private readonly MARKET_EXPRESSContext _context;
        private readonly IConfiguration _configuration;
        private readonly IArticleRepository _articleRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;
        #endregion

        #region CONSTRUCTOR
        public UnitOfWork(MARKET_EXPRESSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region PROPERTIES
        public IArticleRepository Article => _articleRepository ?? new ArticleRepository(_context, _configuration);
        public IClientRepository Client => _clientRepository ?? new ClientRepository(_context, _configuration);
        public IAppUserRepository AppUser => _appUserRepository ?? new AppUserRepository(_context, _configuration);
        public ICartRepository Cart => _cartRepository ?? new CartRepository(_context, _configuration);
        public IAddressRepository Address => _addressRepository ?? new AddressRepository(_context, _configuration);
        #endregion

        #region METHODS
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollBackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
