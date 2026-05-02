namespace Product.Models;

public record ProductCreateRequest(string Name, double Price, int? Stock);
public record ProductUpdateRequest(string? Name, double? Price, int? Stock);
