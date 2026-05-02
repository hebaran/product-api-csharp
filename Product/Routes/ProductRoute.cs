using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.Models;

namespace Product.Routes;

public static class ProductRoute
{
    public static void ProductRoutes(this WebApplication app)
    {
        var productsRoute = app.MapGroup("/products");

        productsRoute.MapPost("/",
        async (ProductCreateRequest request, ProductContext context) =>
        {
            string productName = request.Name;
            double productPrice = request.Price;
            int productStock = request.Stock ?? 0;

            var product = new ProductModel(productName, productPrice, productStock);
            
            await context.AddAsync(product);
            await context.SaveChangesAsync();

            return Results.Created($"/products/{product.Id}", product);
        });

        productsRoute.MapGet("/",
        async (ProductContext context) =>
        {
            var products = await context.Products.ToListAsync();
            
            return Results.Ok(products);
        });

        productsRoute.MapGet("/{id:guid}",
        async (Guid id, ProductContext context) =>
        {
            var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (product == null) { return Results.NotFound(); }

            return Results.Ok(product);
        });

        productsRoute.MapPatch("/{id:guid}",
        async (Guid id, ProductUpdateRequest request, ProductContext context) =>
        {
            var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (product == null) { return Results.NotFound(); }
            
            string? productName = request.Name;
            double? productPrice = request.Price;
            int? productStock = request.Stock;
            
            if (!string.IsNullOrWhiteSpace(productName)) { product.ChangeName(productName); }
            if (productPrice.HasValue) { product.ChangePrice(productPrice.Value); }
            if (productStock.HasValue) { product.UpdateStock(productStock.Value); }

            await context.SaveChangesAsync();

            return Results.Ok(product);
        });

        productsRoute.MapDelete("/{id:guid}",
        async (Guid id, ProductContext context) =>
        {
            var product = await context.Products.FirstOrDefaultAsync(product => product.Id == id);

            if (product == null) { return Results.NotFound(); }

            context.Remove(product);
            await context.SaveChangesAsync();

            return Results.NoContent();

        });
    }
}
