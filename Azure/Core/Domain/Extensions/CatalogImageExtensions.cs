using Core.Domain.Entities;
using Core.Domain.Responses;

namespace Core.Domain.Extensions;

public static class CatalogImageExtensions
{
    public static BaseCatalogImageResponse MapToBaseResponse(this CatalogImage entity)
    {
        return new BaseCatalogImageResponse(entity.Id, entity.ProductCategory, entity.BlobName, entity.BlobUrl);
    }
}