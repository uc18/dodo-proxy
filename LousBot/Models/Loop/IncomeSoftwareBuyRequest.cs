using System.Text.Json.Serialization;

namespace LousBot.Models.Loop;

public class IncomeSoftwareBuyRequest : IncomeFormRequest
{
    [JsonPropertyName("submission")]
    public BuySubmission Submission { get; set; }
}