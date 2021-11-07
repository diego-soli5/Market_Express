using Market_Express.Domain.Entities;
using Market_Express.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Cart");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.OpeningDate).HasColumnType("datetime");

            /*builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false);*/

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(e => e.ToString(),
                                    e => (CartStatus)Enum.Parse(typeof(CartStatus), e));

            builder.HasOne(d => d.Client)
                .WithMany(p => p.Carts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cart__ClientId__5535A963");
        }
    }
}
