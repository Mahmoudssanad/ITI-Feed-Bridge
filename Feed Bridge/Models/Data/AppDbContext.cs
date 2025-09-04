using Feed_Bridge.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // int مش string ك database يتشاف في ال Order Enum عملت كدا عشان ال 
            builder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion<string>();

        }

    }
}
