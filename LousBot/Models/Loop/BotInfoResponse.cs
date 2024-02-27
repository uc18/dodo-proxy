using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public record BotInfoResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}