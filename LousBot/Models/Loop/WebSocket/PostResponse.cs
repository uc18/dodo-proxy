using System;
using System.Text.Json.Serialization;

namespace LousBot.Models.Loop.WebSocket;

[Serializable]
public record PostResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("root_id")]
    public string RootId { get; set; }
}