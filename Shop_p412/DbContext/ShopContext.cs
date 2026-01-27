using Microsoft.EntityFrameworkCore;

public class ShopContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //...
    }

}
