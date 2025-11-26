namespace Core.Domain.Responses;

public class BaseCatalogResponse
{
    public string BlobName { get; set; }
    public string BlobUrl { get; set; }

    public BaseCatalogResponse(string blobName, string blobUrl)
    {
        BlobName = blobName;
        BlobUrl = blobUrl;
    }
}