using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class BinacleAccessConfigurations : IEntityTypeConfiguration<BinnacleAccess>
    {
        public void Configure(EntityTypeBuilder<BinnacleAccess> builder)
        {
            builder.ToTable("Binnacle_Access");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.EntryDate).HasColumnType("datetime");

            builder.Property(e => e.ExitDate).HasColumnType("datetime");

            builder.HasOne(d => d.AppUser)
                .WithMany(p => p.BinnacleAccesses)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Binnacle___AppUs__440B1D61");
        }
    }
}
