using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BinaccleMovementConfigurations : IEntityTypeConfiguration<BinnacleMovement>
    {
        public void Configure(EntityTypeBuilder<BinnacleMovement> builder)
        {
            builder.ToTable("Binnacle_Movement");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Detail)
                .IsRequired()
                .HasMaxLength(600)
                .IsUnicode(false);

            builder.Property(e => e.PerformedBy)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.MovementDate).HasColumnType("datetime");

            builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);
        }
    }
}
