using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class Submission
{
    [JsonPropertyName("justification")]
    public string Justification { get; set; }

    [JsonPropertyName("service")]
    public string Service { get; set; }
}