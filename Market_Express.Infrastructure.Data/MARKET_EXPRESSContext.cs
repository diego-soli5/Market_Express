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


        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<AppUserRole> AppUserRole { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<BinnacleAccess> BinnacleAccess { get; set; }
        public virtual DbSet<BinnacleMovement> BinnacleMovement { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartDetail> CartDetail { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
