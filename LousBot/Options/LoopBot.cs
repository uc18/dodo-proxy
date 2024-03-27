namespace LousBot.Options;

public record LoopBot
{
    public string ApiUrl { get; init; }

    public string WebsocketUrl { get; init; }

    public string ReturnUrl { get; init; }

    public string BotName { get; init; }

    public string BotToken { get; init; }

    public string BearerToken { get; init; }
}