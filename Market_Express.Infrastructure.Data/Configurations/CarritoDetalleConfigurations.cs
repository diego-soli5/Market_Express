using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class CarritoDetalleConfigurations : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.ToTable("Carrito_Detalle");

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.ArticleId).HasColumnName("Id_Articulo");

            builder.Property(e => e.CartId).HasColumnName("Id_Carrito");

            builder.HasOne(d => d.Article)
                .WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_D__Id_Ar__571DF1D5");

            builder.HasOne(d => d.Cart)
                .WithMany(p => p.CartDetails)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carrito_D__Id_Ca__5629CD9C");
        }
    }
}
