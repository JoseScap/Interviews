using Newtonsoft.Json;

namespace Core.Domain.Entities;

public abstract class BaseEntity
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [JsonProperty("_rid")]
    public string Rid { get; set; } = string.Empty;
    [JsonProperty("_self")]
    public string Self { get; set; } = string.Empty;
    [JsonProperty("_etag")]
    public string Etag { get; set; } = string.Empty;
    [JsonProperty("_attachments")]
    public string Attachments { get; set; } = string.Empty;
    [JsonProperty("_ts")]
    public long Ts { get; set; }
}