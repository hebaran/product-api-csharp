namespace Product.Models;

public class ProductModel
{
    public ProductModel(string name, double price, int stock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Stock = stock;
    }

    public Guid Id { get; init; }
    public string Name { get; private set; }
    public double Price { get; private set; }
    public int Stock { get; private set; }

    public void ChangeName(string newName) { Name = newName; }
    public void ChangePrice(double newPrice) { Price = newPrice; }
    public void UpdateStock(int newStock) { Stock = newStock; }
}
