using Microsoft.EntityFrameworkCore;
using Product.Models;

namespace Product.Data;

public class ProductContext : DbContext
{
    public DbSet<ProductModel> Products { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=products.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}
