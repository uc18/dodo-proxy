using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class IncomeFormRequest
{
    [JsonPropertyName("type")]
    public string Type { get; init; }

    [JsonPropertyName("callback_id")]
    public string CallbackId { get; init; }

    [JsonPropertyName("state")]
    public string State { get; init; }

    [JsonPropertyName("user_id")]
    public string UserId { get; init; }

    [JsonPropertyName("channel_id")]
    public string ChannelId { get; init; }

    [JsonPropertyName("team_id")]
    public string TeamId { get; init; }

    //TODO: сделать 2 новых модели в зависимости от запроса, а эту сделать базовой
    [JsonPropertyName("submission")]
    public object Submission { get; init; }
}