using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CarritoConfigurations : IEntityTypeConfiguration<Carrito>
    {
        public void Configure(EntityTypeBuilder<Carrito> builder)
        {
            builder.ToTable("Carrito");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false);

            builder.Property(e => e.FecApertura)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Apertura");

            builder.Property(e => e.IdCliente).HasColumnName("Id_Cliente");

            builder.HasOne(d => d.Cliente)
                .WithMany(p => p.Carrito)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito__Id_Clie__5165187F");
        }
    }
}
