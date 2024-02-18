using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class Element
{
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("help_text")]
    public string HelpText { get; set; }

    [JsonPropertyName("options")]
    public LoopModalOption[] Options { get; set; }
}