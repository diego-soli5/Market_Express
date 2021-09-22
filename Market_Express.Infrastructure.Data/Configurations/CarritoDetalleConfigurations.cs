using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CarritoDetalleConfigurations : IEntityTypeConfiguration<CarritoDetalle>
    {
        public void Configure(EntityTypeBuilder<CarritoDetalle> builder)
        {
            builder.ToTable("Carrito_Detalle");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.IdArticulo).HasColumnName("Id_Articulo");

            builder.Property(e => e.IdCarrito).HasColumnName("Id_Carrito");

            builder.HasOne(d => d.IdArticuloNavigation)
                .WithMany(p => p.CarritoDetalles)
                .HasForeignKey(d => d.IdArticulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_D__Id_Ar__571DF1D5");

            builder.HasOne(d => d.IdCarritoNavigation)
                .WithMany(p => p.CarritoDetalles)
                .HasForeignKey(d => d.IdCarrito)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_D__Id_Ca__5629CD9C");
        }
    }
}
