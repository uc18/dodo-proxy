namespace LousBot.Options;

public record PyrusBot
{
    public string ApiUrl { get; init; }

    public string BotName { get; init; }

    public string Token { get; init; }
}