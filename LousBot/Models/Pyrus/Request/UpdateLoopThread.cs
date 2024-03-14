namespace LousBot.Models.Pyrus.Request;

public record UpdateLoopThread
{
    public string ThreadId { get; set; }

    public string Comment { get; set; }
}