using Microsoft.EntityFrameworkCore;
using Product.Data;
using Product.Models;
using Product.Services;

namespace Product.Routes;

public static class ProductRoute
{
    public static void ProductRoutes(this WebApplication app)
    {
        var productsRoute = app.MapGroup("/products");
        var productsByIdRoute = app.MapGroup("/products/{id:guid}");

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

        productsByIdRoute.MapGet("/",
        async (Guid id, ProductContext context) =>
        {
            var product = await ProductService.GetProduct(id, context);

            if (product is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(product);
        });

        productsByIdRoute.MapPatch("/",
        async (Guid id, ProductUpdateRequest request, ProductContext context) =>
        {
            var product = await ProductService.GetProduct(id, context);

            if (product is null)
            {
                return Results.NotFound();
            }
            
            ProductService.MakeChanges(product, request);

            await context.SaveChangesAsync();

            return Results.Ok(product);
        });

        productsByIdRoute.MapDelete("/",
        async (Guid id, ProductContext context) =>
        {
            var product = await ProductService.GetProduct(id, context);

            if (product is null)
            {
                return Results.NotFound();
            }

            context.Remove(product);
            await context.SaveChangesAsync();

            return Results.NoContent();

        });
    }
}
