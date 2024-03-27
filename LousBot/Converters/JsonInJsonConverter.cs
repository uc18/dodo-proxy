using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LousBot.Converters;

public class JsonInJsonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var innerJson = reader.GetString();
        return innerJson is not null ? JsonSerializer.Deserialize<T>(innerJson, options) : default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var innerJson = JsonSerializer.Serialize(value);
        JsonSerializer.Serialize(writer, innerJson, options);
    }
}