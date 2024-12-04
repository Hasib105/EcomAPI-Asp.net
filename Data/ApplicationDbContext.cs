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

        public DbSet<Item> Items { get; set; } // DbSet for Item model
    }
}
