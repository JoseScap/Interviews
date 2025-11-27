using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Application.Ports.Driven;
using Core.Domain.Responses;
using Infrastructure.Storage.Context;

namespace Infrastructure.Storage;

public class CatalogImageStorageService : ICatalogImageStorageService
{
    private readonly BlobContainerClient _catalogImagesContainer;
    private readonly long _catalogImagesMaxSizeInBytes;

    public CatalogImageStorageService(StorageContext storageContext)
    {
        _catalogImagesContainer = storageContext.CatalogImages;
        _catalogImagesMaxSizeInBytes = storageContext.CatalogImagesMaxSizeInBytes;
    }

    public async Task<(string blobName, string blobUrl)> UploadAsync(
        Stream content,
        string blobName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        if (content.Length > _catalogImagesMaxSizeInBytes)
        {
            var maxKb = _catalogImagesMaxSizeInBytes / 1024;
            var currentKb = content.Length / 1024;
            throw new ArgumentException($"File exceeds the maximum allowed size of {maxKb} KB. Current size: {currentKb} KB.");
        }

        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("Blob name is required.", nameof(blobName));
        }

        var blobClient = _catalogImagesContainer.GetBlobClient(blobName);

        if (content.CanSeek)
        {
            content.Position = 0;
        }

        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = string.IsNullOrWhiteSpace(contentType)
                    ? "application/octet-stream"
                    : contentType
            },
            AccessTier = AccessTier.Hot
        };

        await blobClient.UploadAsync(content, uploadOptions, cancellationToken);

        return (blobClient.Name, blobClient.Uri.ToString());
    }

    public async Task DeleteAsync(
        string blobName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("Blob name is required.", nameof(blobName));
        }

        var blobClient = _catalogImagesContainer.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}

