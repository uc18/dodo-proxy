using System;
using System.Text.Json.Serialization;
using LousBot.Converters;

namespace LousBot.Models.Loop.WebSocket;

[Serializable]
public record DataResponse
{
    [JsonPropertyName("post")]
    [property: JsonConverter(typeof(JsonInJsonConverter<PostResponse>))]
    public PostResponse Post { get; set; }

    [JsonPropertyName("sender_name")]
    public string SenderName { get; set; }
}