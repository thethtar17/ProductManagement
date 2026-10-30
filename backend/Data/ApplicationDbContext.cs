using management.Models;
using Microsoft.EntityFrameworkCore;

namespace management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship
        //    modelBuilder.Entity<Product>()
        //.HasOne(p => p.User)
        //.WithMany(u => u.Products)
        //.HasForeignKey(p => p.UserId);

            // Optional: Configure ImageData as VARBINARY and ImageMimeType as a string
            modelBuilder.Entity<Product>()
                .Property(p => p.ImageData)
                .HasColumnType("VARBINARY(MAX)");

            modelBuilder.Entity<Product>()
                .Property(p => p.ImageMimeType)
                .HasMaxLength(100);

         
            // Configure the one-to-many relationship between Product and User
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between CartItem and Product
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product) // Navigation property to Product
                .WithMany() // Product does not have a collection of CartItems
                .HasForeignKey(ci => ci.ProductId) // Only use ProductId for the foreign key
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Configure the relationship between CartItem and User
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        

    }
    }

}
