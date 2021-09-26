using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BinacleAccessConfigurations : IEntityTypeConfiguration<BinnacleAccess>
    {
        public void Configure(EntityTypeBuilder<BinnacleAccess> builder)
        {
            builder.ToTable("Bitacora_Acceso");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.EntryDate)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");

            builder.Property(e => e.ExitDate)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Salida");

            builder.Property(e => e.UserId).HasColumnName("Id_Usuario");

            builder.HasOne(d => d.AppUser)
                .WithMany(p => p.BinnacleAccess)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora___Id_Us__4222D4EF");
        }
    }
}
