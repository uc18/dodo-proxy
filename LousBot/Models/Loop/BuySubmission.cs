using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class BuySubmission : Submission
{
    [JsonPropertyName("buyservicename")]
    public string BuyServiceName { get; set; }
}