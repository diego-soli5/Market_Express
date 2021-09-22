using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class UsuarioRolConfigurations : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.ToTable("Usuario_Rol");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.UsuarioRols)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario_R__Id_Us__36B12243");
        }
    }
}
