using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BitacoraAccesoConfigurations : IEntityTypeConfiguration<BitacoraAcceso>
    {
        public void Configure(EntityTypeBuilder<BitacoraAcceso> builder)
        {
            builder.ToTable("Bitacora_Acceso");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Inicio");

            builder.Property(e => e.FechaSalida)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Salida");

            builder.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            builder.HasOne(d => d.IdUsuarioNavigation)
                .WithMany(p => p.BitacoraAccesos)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora___Id_Us__4222D4EF");
        }
    }
}
