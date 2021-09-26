using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Pedido");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(9)
                .IsUnicode(false);

            builder.Property(e => e.CreationDate)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");

            builder.Property(e => e.ClientId).HasColumnName("Id_Cliente");

            builder.Property(e => e.Total).HasColumnType("decimal(19, 2)");
        }
    }
}
