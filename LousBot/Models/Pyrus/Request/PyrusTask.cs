using System.Text.Json.Nodes;

namespace LousBot.Models.Pyrus.Request;

public record PyrusTask
{
    public JsonObject Task { get; init; }
}