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
        private readonly ISliderRepository _sliderRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IBinnacleAccessRepository _binnacleAccessRepository;
        private readonly IBinnacleMovementRepository _binnacleMovementRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAppUserRoleRepository _appUserRoleRepository;
        private readonly ICartDetailRepository _cartDetailRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IReportRepository _reportRepository;
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
        public ISliderRepository Slider => _sliderRepository ?? new SliderRepository(_context, _configuration);
        public ICategoryRepository Category => _categoryRepository ?? new CategoryRepository(_context, _configuration);
        public IRoleRepository Role => _roleRepository ?? new RoleRepository(_context, _configuration);
        public IBinnacleAccessRepository BinnacleAccess => _binnacleAccessRepository ?? new BinnacleAccessRepository(_context, _configuration);
        public IBinnacleMovementRepository BinnacleMovement => _binnacleMovementRepository ?? new BinnacleMovementRepository(_context, _configuration);
        public IOrderRepository Order => _orderRepository ?? new OrderRepository(_context, _configuration);
        public IAppUserRoleRepository AppUserRole => _appUserRoleRepository ?? new AppUserRoleRepository(_context, _configuration);
        public ICartDetailRepository CartDetail => _cartDetailRepository ?? new CartDetailRepository(_context, _configuration);
        public IOrderDetailRepository OrderDetail => _orderDetailRepository ?? new OrderDetailRepository(_context, _configuration);
        public IPermissionRepository Permission => _permissionRepository ?? new PermissionRepository(_context, _configuration);
        public IRolePermissionRepository RolePermission => _rolePermissionRepository ?? new RolePermissionRepository(_context, _configuration);
        public IReportRepository Report => _reportRepository ?? new ReportRepository(_configuration);
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
