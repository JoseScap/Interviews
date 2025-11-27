using Newtonsoft.Json;

namespace Core.Domain.Entities;

public class Product : BaseEntity
{
    public string ProductCategory { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public Product()
    {
    }

    public Product(
        string productCategory, string name, double price,
        string description)
    {
        ProductCategory = productCategory;
        Name = name;
        Price = price;
        Description = description;
    }

    public Product(
        string id, string productCategory, string name,
        double price, string description)
    {
        Id = Guid.Parse(id).ToString();
        ProductCategory = productCategory;
        Name = name;
        Price = price;
        Description = description;
    }

    public static string PartitionKeyPath => $"/{nameof(ProductCategory)}";
    public static string ContainerName => $"{nameof(Product)}s";
}
