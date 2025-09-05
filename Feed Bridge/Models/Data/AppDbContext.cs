using Feed_Bridge.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace Feed_Bridge.Models.Data
{
    public class AppDbContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Support> Supports { get; set; }
        public DbSet<StaticPage> StaticPages { get; set; }
        public DbSet<Partener> Parteners { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ProductCart> ProductCarts { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // int مش string ك database يتشاف في ال Order Enum عملت كدا عشان ال 
            builder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion<string>();

            builder.Entity<ProductCart>().HasOne(x => x.Cart)
                .WithMany(x => x.ProductCarts).HasForeignKey(x => x.CartId);

            builder.Entity<ProductCart>().HasOne(x => x.Product)
               .WithMany(x => x.ProductCarts).HasForeignKey(x => x.ProductId);

            // Composite PK => Id property ال OrderProduct class لاني معملتش في 
            builder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            builder.Entity<OrderProduct>().HasOne(x => x.Order)
                .WithMany(x => x.OrderProducts).HasForeignKey(x => x.OrderId);

            builder.Entity<OrderProduct>().HasOne(x => x.Product)
                .WithMany(x => x.OrderProducts).HasForeignKey(x => x.ProductId);
        }

    }
}
