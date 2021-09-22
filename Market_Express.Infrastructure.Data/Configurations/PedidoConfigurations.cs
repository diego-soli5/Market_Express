using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class PedidoConfigurations : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedido");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(9)
                .IsUnicode(false);

            builder.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");

            builder.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

            builder.Property(e => e.Total).HasColumnType("decimal(19, 2)");
        }
    }
}
