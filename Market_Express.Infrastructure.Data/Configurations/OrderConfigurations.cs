using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("TB_Order");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(9)
                .IsUnicode(false);

            builder.Property(e => e.Total).HasColumnType("decimal(19, 2)");

            builder.HasOne(d => d.Client)
                .WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_Order__Client__5EBF139D");
        }
    }
}
