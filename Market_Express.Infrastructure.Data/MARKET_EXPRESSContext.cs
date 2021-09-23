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

        public DbSet<BitacoraAcceso> BitacoraAcceso { get; set; }
        public DbSet<BitacoraMovimiento> BitacoraMovimiento { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<CarritoDetalle> CarritoDetalle { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<InventarioArticulo> InventarioArticulo { get; set; }
        public DbSet<InventarioCategoria> InventarioCategoria { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalle { get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<RolPermiso> RolPermiso { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioRol> UsuarioRol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
