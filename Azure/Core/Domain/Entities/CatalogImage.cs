using Newtonsoft.Json;

namespace Core.Domain.Entities;

public class CatalogImage : BaseEntity
{
    public string BlobName { get; set; } = string.Empty;
    public string BlobUrl { get; set; } = string.Empty;
    public string ProductCategory { get; set; } = string.Empty;

    public CatalogImage()
    {
    }

    public CatalogImage(string blobName, string blobUrl, string productCategory)
    {
        BlobName = blobName;
        BlobUrl = blobUrl;
        ProductCategory = productCategory;
    }

    public CatalogImage(string id, string blobName, string blobUrl, string productCategory)
    {
        Id = id;
        BlobName = blobName;
        BlobUrl = blobUrl;
        ProductCategory = productCategory;
    }

    public static string PartitionKeyPath => $"/{nameof(ProductCategory)}";
    public static string ContainerName => $"{nameof(CatalogImage)}s";
}
