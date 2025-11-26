using Core.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configuration;

public class ConfigurationContext
{
    public AzureConfiguration Azure { get; }

    public ConfigurationContext(IConfiguration configuration)
    {
        Azure = new AzureConfiguration(configuration);
    }
}
