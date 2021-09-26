using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class RolPermisoConfigurations : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("Rol_Permiso");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.IdPermiso).HasColumnName("Id_Permiso");

            builder.Property(e => e.IdRol).HasColumnName("Id_Rol");

            builder.HasOne(d => d.IdPermisoNavigation)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rol_Permi__Id_Pe__32E0915F");

            builder.HasOne(d => d.IdRolNavigation)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rol_Permi__Id_Ro__31EC6D26");
        }
    }
}
