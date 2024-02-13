using Microsoft.EntityFrameworkCore;
using Product_API_Version_6.Models;

namespace Product_API_Version_6.Database_Setting
{
    public class ShopContext : DbContext
    {
        //Relationship between the entities Properties using dbcontext inherit
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        //Constructor
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                 .HasMany(c => c.Products)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Seed();
        }
    }
}