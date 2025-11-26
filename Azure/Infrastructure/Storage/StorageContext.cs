using System;
using System.Collections.Generic;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Domain.ValueObjects;
using Infrastructure.Configuration;

namespace Infrastructure.Storage;

public class StorageContext
{
    private readonly BlobServiceClient _client;
    private readonly AzureStorageContainers _azureStorageContainers;
    private readonly IReadOnlyDictionary<string, BlobContainerClient> _namedContainers;

    public BlobContainerClient Catalog => _namedContainers[_azureStorageContainers.Catalog.Name];
    public BlobContainerClient Invoices => _namedContainers[_azureStorageContainers.Invoices.Name];
    public long CatalogMaxSizeInBytes => _azureStorageContainers.Catalog.MaxSizeInBytes;
    public long InvoicesMaxSizeInBytes => _azureStorageContainers.Invoices.MaxSizeInBytes;

    public StorageContext(ConfigurationContext configurationContext)
    {
        ArgumentNullException.ThrowIfNull(configurationContext);

        var azure = configurationContext.Azure;

        _client = new BlobServiceClient(
            new Uri(azure.StorageAccountUri),
            new DefaultAzureCredential());
        
        _azureStorageContainers = azure.StorageContainers;

        _namedContainers = new Dictionary<string, BlobContainerClient>(StringComparer.OrdinalIgnoreCase)
        {
            [_azureStorageContainers.Catalog.Name] = EnsureContainer(_azureStorageContainers.Catalog.Name, PublicAccessType.Blob),
            [_azureStorageContainers.Invoices.Name] = EnsureContainer(_azureStorageContainers.Invoices.Name, PublicAccessType.Blob)
        };
    }

    public BlobContainerClient GetContainer(string containerName)
    {
        if (string.IsNullOrWhiteSpace(containerName))
        {
            throw new ArgumentException("Container name cannot be null or empty.", nameof(containerName));
        }

        if (_namedContainers.TryGetValue(containerName, out var existingContainer))
        {
            return existingContainer;
        }

        var catalog = _client.GetBlobContainerClient(containerName);
        catalog.CreateIfNotExists(PublicAccessType.None);
        return catalog;
    }

    private BlobContainerClient EnsureContainer(
        string containerName,
        PublicAccessType publicAccessType)
    {
        var container = _client.GetBlobContainerClient(containerName);
        container.CreateIfNotExists(
            publicAccessType);
        return container;
    }
}
