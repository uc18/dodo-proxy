namespace LousBot.Models.Loop;

public record IncomeAccessRequest
{
    public string channel_id { get; set; } = string.Empty;

    public string channel_name { get; set; } = string.Empty;

    public string command { get; set; } = string.Empty;

    public string user_name { get; set; } = string.Empty;

    public string token { get; set; } = string.Empty;

    public string text { get; set; } = string.Empty;

    public string trigger_id { get; set; } = string.Empty;
}