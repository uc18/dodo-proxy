using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public record IncomeFormRequest
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("callback_id")]
    public string CallbackId { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; set; }

    [JsonPropertyName("team_id")]
    public string TeamId { get; set; }
}