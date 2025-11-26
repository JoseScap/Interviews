using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Application.Ports.Driven;
using Core.Domain.Responses;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly StorageContext _storageContext;

    public StorageService(StorageContext storageContext)
    {
        _storageContext = storageContext;
    }

    public async Task<BaseCatalogResponse> UploadAsync(
        string containerName,
        Stream content,
        string blobName,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentException("Container name is required.", nameof(containerName));
        }

        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        if (string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("Blob name is required.", nameof(blobName));
        }

        var containerClient = _storageContext.GetContainer(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

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

        return new BaseCatalogResponse(blobClient.Name, blobClient.Uri.ToString());
    }
    public async Task<List<BaseCatalogResponse>> GetAllBlobsAsync(string containerName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentException("Container name is required.", nameof(containerName));
        }

        var containerClient = _storageContext.GetContainer(containerName);
        var responses = new List<BaseCatalogResponse>();

        await foreach (var blobItem in containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            responses.Add(new BaseCatalogResponse(blobClient.Name, blobClient.Uri.ToString()));
        }

        return responses;
    }

    public async Task DeleteAsync(
        string containerName,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentException("Container name is required.", nameof(containerName));
        }

        if (string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("Blob name is required.", nameof(blobName));
        }

        var containerClient = _storageContext.GetContainer(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}

