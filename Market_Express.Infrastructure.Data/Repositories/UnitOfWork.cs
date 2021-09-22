using Market_Express.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Market_Express.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region ATTRIBUTES
        private readonly MARKET_EXPRESSContext _context;
        private readonly IConfiguration _configuration;
        #endregion

        #region CONSTRUCTOR
        public UnitOfWork(MARKET_EXPRESSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #endregion

        #region PROPERTIES
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
