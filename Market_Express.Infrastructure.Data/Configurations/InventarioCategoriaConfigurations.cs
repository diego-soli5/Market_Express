using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class InventarioCategoriaConfigurations : IEntityTypeConfiguration<InventarioCategoria>
    {
        public void Configure(EntityTypeBuilder<InventarioCategoria> builder)
        {
            builder.ToTable("Inventario_Categoria");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.FecCreacion).HasColumnType("datetime");

            builder.Property(e => e.AdicionadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.ModificadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);
        }
    }
}
