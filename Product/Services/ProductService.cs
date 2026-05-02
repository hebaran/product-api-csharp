using Microsoft.EntityFrameworkCore;
using Product.Data;

namespace Product.Services;

public class ProductService
{
    private readonly ProductContext _context;

    public ProductService(ProductContext context)
    {
        _context = context;
    }
 
    public async Task<bool> IdVerify(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == id);
        
        if (product == null) { return false; }
        
        return true;
    } 
}
