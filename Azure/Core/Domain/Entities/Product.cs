using Newtonsoft.Json;

namespace Core.Domain.Entities;

public class Product
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Category { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public Product()
    {
    }

    public Product(
        string category, string name, double price,
        string description)
    {
        Category = category;
        Name = name;
        Price = price;
        Description = description;
    }

    public Product(
        string id, string category, string name,
        double price, string description)
    {
        Id = Guid.Parse(id).ToString();
        Category = category;
        Name = name;
        Price = price;
        Description = description;
    }

    public static string PartitionKeyPath => $"/{nameof(Category)}";
}
