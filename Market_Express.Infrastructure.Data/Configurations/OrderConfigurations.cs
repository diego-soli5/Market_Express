using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.CreationDate).HasColumnType("datetime");

            builder.Property(e => e.ShippingAddress)
                    .HasMaxLength(255)
                    .IsRequired();

            builder.Property(e => e.OrderNumber)
                    .ValueGeneratedOnAdd()
                    .IsRequired()
                    .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            /* builder.Property(e => e.Status)
                 .IsRequired()
                 .HasMaxLength(9)
                 .IsUnicode(false);*/

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (OrderStatus)Enum.Parse(typeof(OrderStatus), e));

            builder.Property(e => e.Total).HasColumnType("decimal(19, 2)");

            builder.HasOne(d => d.Client)
                .WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TB_Order__Client__5EBF139D");
        }
    }
}
