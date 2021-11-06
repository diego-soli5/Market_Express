using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class RoleConfigurations : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ModificationDate).HasColumnType("datetime");

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.AddedBy)
                .HasMaxLength(12)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (EntityStatus)Enum.Parse(typeof(EntityStatus), e));
        }
    }
}
