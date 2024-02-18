namespace LousBot.Options;

public record ServiceDeskOptions
{
    public PyrusBot PyrusBotOptions { get; set; }

    public LoopBot LoopBotOptions { get; set; }
}