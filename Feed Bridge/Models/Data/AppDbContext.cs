using Feed_Bridge.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Feed_Bridge.Models.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
<<<<<<< HEAD

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<StaticPage> StaticPages { get; set; }
        public DbSet<Partener> Parteners { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // int مش string ك database يتشاف في ال Order Enum عملت كدا عشان ال 
            builder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion<string>();

        }
=======
>>>>>>> 4570d32ee069cf5a2c94847c58f5b8da98aefa16
    }
}
