using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class IncomeAccessServiceRequest : IncomeFormRequest
{
    [JsonPropertyName("submission")]
    public Submission Submission { get; init; }
}