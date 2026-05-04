using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.Models;

namespace Product.Services;

public static class ProductService
{
    public static async Task<ProductModel?> GetProduct(Guid id, ProductContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == id);

        return product;
    }

    public static void MakeChanges(ProductModel product, ProductUpdateRequest request)
    {
        string? productName = request.Name;
        double? productPrice = request.Price;
        int? productStock = request.Stock;
        
        if (!string.IsNullOrWhiteSpace(productName))
        {
            product.ChangeName(productName);
        }

        if (productPrice.HasValue)
        {
            product.ChangePrice(productPrice.Value);
        }

        if (productStock.HasValue)
        {
            product.UpdateStock(productStock.Value);
        }
    }
}
