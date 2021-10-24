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
        public IAddressRepository Address { get; }
        public ISliderRepository Slider { get; }
        public ICategoryRepository Category { get; }
        public IRoleRepository Role { get; }
        public IBinnacleAccessRepository BinnacleAccess { get; }
        public IBinnacleMovementRepository BinnacleMovement { get; }
        public IOrderRepository Order { get; }
        public IAppUserRoleRepository AppUserRole { get; }
        public ICartDetailRepository CartDetail { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IPermissionRepository Permission { get; }
        public IRolePermissionRepository RolePermission { get; }


        Task BeginTransactionAsync();
        Task RollBackAsync();
        Task CommitTransactionAsync();
        Task<bool> Save();
    }
}
