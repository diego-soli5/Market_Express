using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class PedidoDetalleConfigurations : IEntityTypeConfiguration<PedidoDetalle>
    {
        public void Configure(EntityTypeBuilder<PedidoDetalle> builder)
        {
            builder.ToTable("Pedido_Detalle");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.CodigoBarras)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Codigo_Barras");

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.IdArticulo).HasColumnName("Id_Articulo");

            builder.Property(e => e.IdPedido).HasColumnName("Id_Pedido");

            builder.Property(e => e.Precio).HasColumnType("decimal(19, 2)");

            builder.HasOne(d => d.IdArticuloNavigation)
                .WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdArticulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedido_De__Id_Ar__5FB337D6");

            builder.HasOne(d => d.IdPedidoNavigation)
                .WithMany(p => p.PedidoDetalles)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedido_De__Id_Pe__5EBF139D");
        }
    }
}
