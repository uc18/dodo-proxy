using System.Text.Json.Serialization;

namespace LousBot.Models.Pyrus;

public record CaptureResponse
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("approval_choice")]
    public string ApprovalChoice { get; set; }
}