using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class DirectChannelResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}