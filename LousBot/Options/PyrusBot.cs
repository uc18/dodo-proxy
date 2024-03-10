namespace LousBot.Options;

public record PyrusBot
{
    public string ApiUrl { get; init; }

    public string PyrusUrl { get; init; }

    public string Login { get; init; }

    public string Token { get; init; }
}