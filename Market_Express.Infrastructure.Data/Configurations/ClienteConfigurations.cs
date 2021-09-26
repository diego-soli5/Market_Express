using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class ClienteConfigurations : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Cliente");

            builder.HasIndex(e => e.IdUsuario, "UQ__Cliente__63C76BE34667CEEB")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AutoSinc).HasColumnName("Auto_Sinc");

            builder.Property(e => e.CodCliente)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Cod_Cliente");

            builder.Property(e => e.IdUsuario).HasColumnName("Id_Usuario");

            builder.HasOne(d => d.Usuario)
                .WithOne(p => p.Cliente)
                .HasForeignKey<Client>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cliente__Id_Usua__3B75D760");
        }
    }
}
