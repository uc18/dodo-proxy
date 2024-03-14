using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public record MessageResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }
}