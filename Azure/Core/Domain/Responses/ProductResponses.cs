namespace Core.Domain.Responses;

public class BaseProductResponse
{
    public string Id { get; set; }
    public string ProductCategory { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }

    public BaseProductResponse(
        string id, string productCategory, string name,
        double price, string description)
    {
        Id = id;
        ProductCategory = productCategory;
        Name = name;
        Price = price;
        Description = description;
    }
}