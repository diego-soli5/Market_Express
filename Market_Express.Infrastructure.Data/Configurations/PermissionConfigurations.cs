using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class PermissionConfigurations : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasMaxLength(10)
               .IsUnicode(false);

            builder.Property(e => e.PermissionCode)
               .IsRequired()
               .HasMaxLength(20)
               .IsUnicode(false);

            builder.HasIndex(e => e.PermissionCode)
                .IsUnique();
        }
    }
}
