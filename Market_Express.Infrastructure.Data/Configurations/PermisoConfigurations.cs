using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class PermisoConfigurations : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("Permiso");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
        }
    }
}
