using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class LoopModalOption
{
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }
}