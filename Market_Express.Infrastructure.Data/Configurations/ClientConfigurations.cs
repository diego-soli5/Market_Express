using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market_Express.Infrastructure.Data.Configurations
{
    public class ClientConfigurations : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Cliente");

            builder.HasIndex(e => e.UserId, "UQ__Cliente__63C76BE34667CEEB")
                .IsUnique();

            builder.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            builder.Property(e => e.AutoSync).HasColumnName("Auto_Sinc");

            builder.Property(e => e.ClientCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Cod_Cliente");

            builder.Property(e => e.UserId).HasColumnName("Id_Usuario");

            builder.HasOne(d => d.AppUser)
                .WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Cliente__Id_Usua__3B75D760");
        }
    }
}
