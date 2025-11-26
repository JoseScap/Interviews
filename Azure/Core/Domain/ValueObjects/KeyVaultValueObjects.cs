using Microsoft.Extensions.Configuration;

namespace Core.Domain.ValueObjects;

public sealed class CosmosSecret
{
    public string Value { get; }

    public CosmosSecret(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("CosmosSecret value is missing or not configured.");
        }

        var normalized = value.Trim();

        if (!normalized.Contains("AccountEndpoint=", StringComparison.OrdinalIgnoreCase) ||
            !normalized.Contains("AccountKey=", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("CosmosSecret must contain AccountEndpoint and AccountKey.");
        }

        Value = normalized;
    }
}
