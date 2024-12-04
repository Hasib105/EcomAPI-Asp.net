using Microsoft.EntityFrameworkCore;
using EcomApi.Models;

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
    }
}