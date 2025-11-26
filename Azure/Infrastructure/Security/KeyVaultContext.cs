using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.Configuration;

namespace Infrastructure.Security;

public class KeyVaultContext
{
    private readonly SecretClient _secretClient;
    public KeyVaultSecret CosmosSecret => _secretClient.GetSecret("CosmosSecret");
    public KeyVaultSecret StorageSecret => _secretClient.GetSecret("StorageSecret");
    
    public KeyVaultContext(ConfigurationContext configurationContext)
    {
        _secretClient = new SecretClient(
            new Uri(configurationContext.Azure.KeyVaultUri),
            new DefaultAzureCredential());
    }

    
}
