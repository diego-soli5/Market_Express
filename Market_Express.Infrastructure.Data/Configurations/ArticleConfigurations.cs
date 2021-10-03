using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Market_Express.Infrastructure.Data.Configurations
{
    public class ArticleConfigurations : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("Article");

            builder.HasIndex(e => e.BarCode, "UQ__Article__8A2ACA9BA865F7D6")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AddedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.BarCode)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ModificationDate).HasColumnType("datetime");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);

            builder.Property(e => e.Image)
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.ModifiedBy)
                .HasMaxLength(12)
                .IsUnicode(false);

            builder.Property(e => e.Price).HasColumnType("decimal(19, 2)");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.HasOne(d => d.Category)
                .WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Article__Categor__5070F446");
        }
    }
}
