using System.Reflection;
using Market_Express.Domain.Entities;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Market_Express.Infrastructure.Data
{
    public class MARKET_EXPRESSContext : DbContext
    {
        public MARKET_EXPRESSContext()
        {
        }

        public MARKET_EXPRESSContext(DbContextOptions<MARKET_EXPRESSContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<AppUserRole> AppUserRole { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<BinnacleAccess> BinnacleAccess { get; set; }
        public DbSet<BinnacleMovement> BinnacleMovement { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
