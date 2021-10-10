using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class SliderConfigurations : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.ToTable("Slider");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Image)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (EntityStatus)Enum.Parse(typeof(EntityStatus), e));

           /* builder.Property(e => e.Status)
                 .IsRequired()
                 .HasMaxLength(11)
                 .IsUnicode(false);
           */

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ModificationDate).HasColumnType("datetime");

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(40)
                .IsUnicode(false);

            builder.Property(e => e.AddedBy)
                .HasMaxLength(40)
                .IsUnicode(false);
        }
    }
}
