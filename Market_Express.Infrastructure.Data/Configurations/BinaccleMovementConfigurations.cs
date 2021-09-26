using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BinaccleMovementConfigurations : IEntityTypeConfiguration<BinnacleMovement>
    {
        public void Configure(EntityTypeBuilder<BinnacleMovement> builder)
        {
            builder.ToTable("Bitacora_Movimiento");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Detail)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.MovementDate).HasColumnType("datetime");

            builder.Property(e => e.UserId).HasColumnName("Id_Usuario");

            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.HasOne(d => d.AppUser)
                .WithMany(p => p.BinnacleMovements)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Bitacora___Id_Us__45F365D3");
        }
    }
}
