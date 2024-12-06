using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EcomApi.Models;

namespace EcomApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure unique indexes for Email and UserName
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_ApplicationUser_Email");

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_ApplicationUser_UserName");

            // Additional model configurations can go here
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
        public DbSet<Category> Categories { get; set; }

        public override int SaveChanges()
        {
            var categoryEntities = ChangeTracker.Entries<Category>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entity in categoryEntities)
            {
                entity.Entity.GenerateSlug(); // Call the slug generation method
            }

            var productEntities = ChangeTracker.Entries<Product>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entity in productEntities)
            {
                entity.Entity.GenerateSlug(); // Call the slug generation method
            }

            return base.SaveChanges();
        }
    }
}