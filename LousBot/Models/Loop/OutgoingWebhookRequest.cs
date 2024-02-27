using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class OutgoingWebhookRequest
{
    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}