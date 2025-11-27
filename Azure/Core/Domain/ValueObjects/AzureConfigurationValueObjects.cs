using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Core.Domain.ValueObjects;

public sealed class AzureConfiguration
{
    public string KeyVaultUri { get; }
    public string StorageAccountUri { get; }
    public AzureStorageContainers StorageContainers { get; }

    public AzureConfiguration(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection("Azure");

        if (!section.Exists())
        {
            throw new ArgumentException("Azure configuration section is missing.");
        }

        KeyVaultUri = ValidateHttpsUri(section["KeyVaultUri"], "Azure:KeyVaultUri");
        StorageAccountUri = ValidateHttpsUri(section["StorageAccountUri"], "Azure:StorageAccountUri");
        StorageContainers = new AzureStorageContainers(section.GetSection("StorageContainers"));
    }

    private static string ValidateHttpsUri(string? value, string configKey)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{configKey} value is missing or not configured.");
        }

        var normalizedValue = value.Trim();

        if (!Uri.TryCreate(normalizedValue, UriKind.Absolute, out var uri))
        {
            throw new ArgumentException($"{configKey} must be a valid absolute URI.");
        }

        if (!string.Equals(uri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"{configKey} must use HTTPS.");
        }

        return uri.ToString();
    }
}

public sealed class AzureStorageContainers
{
    public AzureStorageContainerDefinition CatalogImages { get; }
    public AzureStorageContainerDefinition Invoices { get; }

    public AzureStorageContainers(IConfigurationSection configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (!configuration.Exists())
        {
            throw new ArgumentException("Azure:StorageContainers section is missing.");
        }

        CatalogImages = CreateDefinition(configuration, "CatalogImages", "CatalogImagesMaxKbSize");
        Invoices = CreateDefinition(configuration, "Invoices", "InvoicesMaxKbSize");
    }

    private static AzureStorageContainerDefinition CreateDefinition(
        IConfigurationSection configuration,
        string nameKey,
        string sizeKey)
    {
        var containerName = configuration[nameKey];
        var maxSizeValue = configuration[sizeKey];

        var name = ValidateContainerName(containerName, $"Azure:StorageContainers:{nameKey}");

        if (string.IsNullOrWhiteSpace(maxSizeValue) ||
            !long.TryParse(maxSizeValue, out var maxSizeKb) ||
            maxSizeKb <= 0)
        {
            throw new ArgumentException($"Azure:StorageContainers:{sizeKey} must be a positive integer (KB).");
        }

        var maxSizeBytes = checked(maxSizeKb * 1024L);

        return new AzureStorageContainerDefinition(name, maxSizeBytes);
    }

    private static string ValidateContainerName(string? value, string configKey)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{configKey} value is missing or not configured.");
        }

        var normalized = value.Trim().ToLowerInvariant();

        if (!Regex.IsMatch(normalized, "^[a-z0-9](?:[a-z0-9\\-]*[a-z0-9])?$") ||
            normalized.Length < 3 ||
            normalized.Length > 63)
        {
            throw new ArgumentException($"{configKey} must be a valid blob container name.");
        }

        return normalized;
    }
}

public sealed record AzureStorageContainerDefinition(string Name, long MaxSizeInBytes);