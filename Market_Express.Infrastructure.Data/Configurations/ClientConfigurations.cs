using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class ClientConfigurations : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasIndex(e => e.AppUserId, "UQ__Client__FC65C731D41FFC29")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.ClientCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.HasOne(d => d.AppUser)
                .WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Client__AppUserI__3C69FB99");
        }
    }
}
