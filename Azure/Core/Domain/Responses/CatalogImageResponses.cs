namespace Core.Domain.Responses;

public class BaseCatalogImageResponse
{
    public string Id { get; set; }
    public string ProductCategory { get; set; }
    public string BlobName { get; set; }
    public string BlobUrl { get; set; }

    public BaseCatalogImageResponse(string id, string productCategory, string blobName, string blobUrl)
    {
        Id = id;
        ProductCategory = productCategory;
        BlobName = blobName;
        BlobUrl = blobUrl;
    }
}