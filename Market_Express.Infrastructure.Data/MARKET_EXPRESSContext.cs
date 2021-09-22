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

        public virtual DbSet<BitacoraAcceso> BitacoraAcceso { get; set; }
        public virtual DbSet<BitacoraMovimiento> BitacoraMovimiento { get; set; }
        public virtual DbSet<Carrito> Carrito { get; set; }
        public virtual DbSet<CarritoDetalle> CarritoDetalle { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Direccion> Direccion { get; set; }
        public virtual DbSet<InventarioArticulo> InventarioArticulo { get; set; }
        public virtual DbSet<InventarioCategoria> InventarioCategoria { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<PedidoDetalle> PedidoDetalle { get; set; }
        public virtual DbSet<Permiso> Permiso { get; set; }
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<RolPermiso> RolPermiso { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
