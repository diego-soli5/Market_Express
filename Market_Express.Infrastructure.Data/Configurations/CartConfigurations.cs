using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carrito");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(7)
                .IsUnicode(false);

            builder.Property(e => e.OpeningDate)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Apertura");

            builder.Property(e => e.ClientId).HasColumnName("Id_Cliente");

            builder.HasOne(d => d.Client)
                .WithMany(p => p.Cart)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito__Id_Clie__5165187F");
        }
    }
}
