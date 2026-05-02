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
        var productsByIdRoute = app.MapGroup("/products/{id:guid}")
            .AddEndpointFilter(async (context, next) =>
            {
                var productService = context.HttpContext.RequestServices.GetRequiredService<ProductService>();
                var requestId = context.GetArgument<Guid>(0);
                var idExists = await productService.IdVerify(requestId);

                if (!idExists) { return Results.NotFound(); }

                var result = await next(context);

                return result;
            });

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
            var product = await context.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == id);

            return Results.Ok(product);
        });

        productsByIdRoute.MapPatch("/",
        async (Guid id, ProductUpdateRequest request, ProductContext context) =>
        {
            var product = await context.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == id);
            
            string? productName = request.Name;
            double? productPrice = request.Price;
            int? productStock = request.Stock;
            
            if (!string.IsNullOrWhiteSpace(productName)) { product.ChangeName(productName); }
            if (productPrice.HasValue) { product.ChangePrice(productPrice.Value); }
            if (productStock.HasValue) { product.UpdateStock(productStock.Value); }

            await context.SaveChangesAsync();

            return Results.Ok(product);
        });

        productsByIdRoute.MapDelete("/",
        async (Guid id, ProductContext context) =>
        {
            var product = await context.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == id);

            context.Remove(product);
            await context.SaveChangesAsync();

            return Results.NoContent();

        });
    }
}
