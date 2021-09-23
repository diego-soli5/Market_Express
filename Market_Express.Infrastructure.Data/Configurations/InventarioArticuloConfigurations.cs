using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Market_Express.Infrastructure.Data.Configurations
{
    public class InventarioArticuloConfigurations : IEntityTypeConfiguration<InventarioArticulo>
    {
        public void Configure(EntityTypeBuilder<InventarioArticulo> builder)
        {
            builder.ToTable("Inventario_Articulo");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AutoSinc).HasColumnName("Auto_Sinc");

            builder.Property(e => e.CodigoBarras)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Codigo_Barras");

            builder.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(e => e.IdCategoria).HasColumnName("Id_Categoria");

            builder.Property(e => e.Imagen)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Precio).HasColumnType("decimal(19, 2)");

            builder.Property(e => e.AdicionadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.ModificadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);
        }
    }
}
