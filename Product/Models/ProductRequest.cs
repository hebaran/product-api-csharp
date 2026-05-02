namespace Product.Models;

public record ProductRequest(string Name, double Price, int? Stock = 0);
