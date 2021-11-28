using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");

            builder.HasIndex(e => e.Identification, "UQ__AppUser__724F06FD13D4DBD5")
                .IsUnique();

            builder.HasIndex(e => e.Email, "UQ__AppUser__A9D105342A56E533")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AddedBy)
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ModificationDate).HasColumnType("datetime");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Identification)
                .IsRequired()
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Alias)
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(80)
                .IsUnicode(false);

            builder.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(40)
                .IsUnicode(false);

            /*builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);*/

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (EntityStatus)Enum.Parse(typeof(EntityStatus), e));

            /*builder.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);*/

            builder.Property(e => e.Type)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (AppUserType)Enum.Parse(typeof(AppUserType), e));
        }
    }
}
