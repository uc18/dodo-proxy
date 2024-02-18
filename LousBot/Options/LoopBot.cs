namespace LousBot.Options;

public record LoopBot
{
    public string ApiUrl { get; init; }

    public string ReturnUrl { get; init; }

    public string Login { get; init; }

    public string BotToken { get; init; }

    public string BearerToken { get; init; }
}