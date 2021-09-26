using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Market_Express.Infrastructure.Data.Configurations
{
    public class InventarioArticuloConfigurations : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Inventario_Articulo");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AutoSync).HasColumnName("Auto_Sinc");

            builder.Property(e => e.BarCode)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Codigo_Barras");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(e => e.CategoryId).HasColumnName("Id_Categoria");

            builder.Property(e => e.Image)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Price).HasColumnType("decimal(19, 2)");

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.AddedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(12)
                .IsUnicode(false);
        }
    }
}
