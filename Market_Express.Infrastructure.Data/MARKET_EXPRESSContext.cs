using System;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public virtual DbSet<BitacoraAcceso> BitacoraAccesos { get; set; }
        public virtual DbSet<BitacoraMovimiento> BitacoraMovimientos { get; set; }
        public virtual DbSet<Carrito> Carritos { get; set; }
        public virtual DbSet<CarritoDetalle> CarritoDetalles { get; set; }
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Direccion> Direccions { get; set; }
        public virtual DbSet<InventarioArticulo> InventarioArticulos { get; set; }
        public virtual DbSet<InventarioCategoria> InventarioCategoria { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<RolPermiso> RolPermisos { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<UsuarioRol> UsuarioRols { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;DataBase=MARKET_EXPRESS;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<BitacoraAcceso>(entity =>
            {
                entity.ToTable("Bitacora_Acceso");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Inicio");

                entity.Property(e => e.FechaSalida)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Salida");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.BitacoraAccesos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bitacora___Id_Us__4222D4EF");
            });

            modelBuilder.Entity<BitacoraMovimiento>(entity =>
            {
                entity.ToTable("Bitacora_Movimiento");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.BitacoraMovimientos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Bitacora___Id_Us__45F365D3");
            });

            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.ToTable("Carrito");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(7)
                    .IsUnicode(false);

                entity.Property(e => e.FechaApertura)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Apertura");

                entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Carritos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carrito__Id_Clie__5165187F");
            });

            modelBuilder.Entity<CarritoDetalle>(entity =>
            {
                entity.ToTable("Carrito_Detalle");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.IdArticulo).HasColumnName("Id_Articulo");

                entity.Property(e => e.IdCarrito).HasColumnName("Id_Carrito");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.CarritoDetalles)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carrito_D__Id_Ar__571DF1D5");

                entity.HasOne(d => d.IdCarritoNavigation)
                    .WithMany(p => p.CarritoDetalles)
                    .HasForeignKey(d => d.IdCarrito)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Carrito_D__Id_Ca__5629CD9C");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.HasIndex(e => e.IdUsuario, "UQ__Cliente__63C76BE34667CEEB")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AutoSinc).HasColumnName("Auto_Sinc");

                entity.Property(e => e.CodCliente)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Cod_Cliente");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithOne(p => p.Cliente)
                    .HasForeignKey<Cliente>(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cliente__Id_Usua__3B75D760");
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.ToTable("Direccion");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Detalle)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InventarioArticulo>(entity =>
            {
                entity.ToTable("Inventario_Articulo");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AutoSinc).HasColumnName("Auto_Sinc");

                entity.Property(e => e.CodigoBarras)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Codigo_Barras");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Precio).HasColumnType("decimal(19, 2)");
            });

            modelBuilder.Entity<InventarioCategoria>(entity =>
            {
                entity.ToTable("Inventario_Categoria");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedido");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Creacion");

                entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

                entity.Property(e => e.Total).HasColumnType("decimal(19, 2)");
            });

            modelBuilder.Entity<PedidoDetalle>(entity =>
            {
                entity.ToTable("Pedido_Detalle");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CodigoBarras)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Codigo_Barras");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdArticulo).HasColumnName("Id_Articulo");

                entity.Property(e => e.IdPedido).HasColumnName("Id_Pedido");

                entity.Property(e => e.Precio).HasColumnType("decimal(19, 2)");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.PedidoDetalles)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pedido_De__Id_Ar__5FB337D6");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.PedidoDetalles)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pedido_De__Id_Pe__5EBF139D");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.ToTable("Permiso");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.ToTable("Rol_Permiso");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.IdPermiso).HasColumnName("Id_Permiso");

                entity.Property(e => e.IdRol).HasColumnName("Id_Rol");

                entity.HasOne(d => d.IdPermisoNavigation)
                    .WithMany(p => p.RolPermisos)
                    .HasForeignKey(d => d.IdPermiso)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rol_Permi__Id_Pe__32E0915F");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolPermisos)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Rol_Permi__Id_Ro__31EC6D26");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534AA11DB4E")
                    .IsUnique();

                entity.HasIndex(e => e.Cedula, "UQ__Usuario__B4ADFE3861767C97")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.ToTable("Usuario_Rol");

                entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuarioRols)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Usuario_R__Id_Us__36B12243");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
