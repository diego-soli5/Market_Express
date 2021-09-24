using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BitacoraMovimientoConfigurations : IEntityTypeConfiguration<BitacoraMovimiento>
    {
        public void Configure(EntityTypeBuilder<BitacoraMovimiento> builder)
        {
            builder.ToTable("Bitacora_Movimiento");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Detalle)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Fecha).HasColumnType("datetime");

            builder.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.BitacoraMovimiento)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora___Id_Us__45F365D3");
        }
    }
}
