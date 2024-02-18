using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class Dialog
{
    [JsonPropertyName("callback_id")]
    public string CallbackId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("elements")]
    public Element[] Elements { get; set; }

    [JsonPropertyName("sumbit_label")]
    public string SumbitLabel { get; set; }
}