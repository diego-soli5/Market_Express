using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class DireccionConfigurations : IEntityTypeConfiguration<Direccion>
    {
        public void Configure(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("Direccion");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Detalle)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
