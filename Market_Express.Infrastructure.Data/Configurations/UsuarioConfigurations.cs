using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class UsuarioConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("Usuario");

            builder.HasIndex(e => e.Email, "UQ__Usuario__A9D10534AA11DB4E")
                .IsUnique();

            builder.HasIndex(e => e.Cedula, "UQ__Usuario__B4ADFE3861767C97")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Cedula)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.Clave)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.FecCreacion).HasColumnType("datetime");

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Telefono)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Tipo)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.AdicionadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.ModificadoPor)
                .HasMaxLength(12)
                .IsUnicode(false);
        }
    }
}
