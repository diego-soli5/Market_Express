using System.Reflection;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Market_Express.Infrastructure.Data
{
    public partial class MARKET_EXPRESSContext : DbContext
    {
        public MARKET_EXPRESSContext()
        {
        }

        public MARKET_EXPRESSContext(DbContextOptions<MARKET_EXPRESSContext> options)
            : base(options)
        {
        }

        public DbSet<BinnacleAccess> BitacoraAcceso { get; set; }
        public DbSet<BinnacleMovement> BitacoraMovimiento { get; set; }
        public DbSet<Cart> Carrito { get; set; }
        public DbSet<CartDetail> CarritoDetalle { get; set; }
        public DbSet<Client> Cliente { get; set; }
        public DbSet<Address> Direccion { get; set; }
        public DbSet<Article> InventarioArticulo { get; set; }
        public DbSet<Category> InventarioCategoria { get; set; }
        public DbSet<Order> Pedido { get; set; }
        public DbSet<OrderDetail> PedidoDetalle { get; set; }
        public DbSet<Permission> Permiso { get; set; }
        public DbSet<Role> Rol { get; set; }
        public DbSet<RolePermission> RolPermiso { get; set; }
        public DbSet<AppUser> Usuario { get; set; }
        public DbSet<AppUserRole> UsuarioRol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
