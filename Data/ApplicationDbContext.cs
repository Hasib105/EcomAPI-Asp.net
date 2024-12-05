using Microsoft.EntityFrameworkCore;
using EcomApi.Models;
using System.Linq;


namespace EcomApi.Data
{
    public class ApplicationDbContext : DbContext
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
                .HasDatabaseName("IX_ApplicationUser_Email"); // Optional: specify index name

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_ApplicationUser_UserName"); // Optional: specify index name

            // Additional model configurations can go here
        }

        public DbSet<Item> Items { get; set; } // DbSet for Item model
        public DbSet<Product> Products { get; set; } // DbSet for Product model
        public DbSet<ProductImage> ProductImages { get; set; } // DbSet for ProductImage model
        public DbSet<Order> Orders { get; set; } // DbSet for Order model
        public DbSet<CartItem> CartItem { get; set; } 
        public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
        public DbSet<Category> Categories { get; set; }


        public override int SaveChanges()
        {
            var category_entities = ChangeTracker.Entries<Category>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entity in category_entities)
            {
                entity.Entity.GenerateSlug(); // Call the slug generation method
            }

            var product_entities = ChangeTracker.Entries<Product>()
                .Where(e => e.State == EntityState.Added);

            foreach (var entity in product_entities)
            {
                entity.Entity.GenerateSlug(); // Call the slug generation method
            }

            return base.SaveChanges();
        }

    }
}