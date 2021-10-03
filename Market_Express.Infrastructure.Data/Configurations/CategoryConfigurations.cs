using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AddedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ModificationDate).HasColumnType("datetime");

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(e => e.Image)
                .HasMaxLength(30)
                .IsUnicode(false);
        }
    }
}
