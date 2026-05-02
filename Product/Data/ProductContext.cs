using Microsoft.EntityFrameworkCore;

namespace Product.Data;

public class ProductContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=products.sqlite");
        base.OnConfiguring(optionsBuilder);
    }
}
