using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class ModalForm
{
    [JsonPropertyName("trigger_id")]
    public string TriggerId { get; set; }

    [JsonPropertyName("url")]
    public string ApiUrl { get; set; }

    [JsonPropertyName("dialog")]
    public Dialog Dialog { get; set; }
}