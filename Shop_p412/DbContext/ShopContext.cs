using Microsoft.EntityFrameworkCore;
using Shop_app_p32.Models;

public class ShopContext : DbContext
{

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // 1-1 User -> Cart
        builder.Entity<ShopUser>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique review per user per product
        builder.Entity<Review>()
            .HasIndex(r => new { r.UserId, r.ProductId })
            .IsUnique();

        // Decimal precision (додатковий захист)
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Entity<OrderItem>()
            .Property(o => o.Price)
            .HasPrecision(18, 2);

        builder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(18, 2);
    }

}
