using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class Submission
{
    [JsonPropertyName("Justification")]
    public string Justification { get; set; }

    [JsonPropertyName("Service")]
    public string Service { get; set; }

    [JsonPropertyName("AnotherNameService")]
    public string? AnotherNameService { get; set; }
}