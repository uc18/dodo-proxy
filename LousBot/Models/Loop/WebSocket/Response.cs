using System;
using System.Text.Json.Serialization;

namespace LousBot.Models.Loop.WebSocket;

[Serializable]
public record Response
{
    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("data")]
    public DataResponse Data { get; set; }
}