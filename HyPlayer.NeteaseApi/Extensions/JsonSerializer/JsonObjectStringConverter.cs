using System.Text.Json;
using System.Text.Json.Serialization;

namespace HyPlayer.NeteaseApi.Extensions.JsonSerializer;

public class JsonObjectStringConverter : JsonConverter<JsonObjectStringWrapper>
{
    public override JsonObjectStringWrapper? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            return new JsonObjectStringWrapper(value);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var json = JsonDocument.ParseValue(ref reader).RootElement;
            return new JsonObjectStringWrapper(json.ToString());
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return new JsonObjectStringWrapper(null);
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return new JsonObjectStringWrapper(reader.GetInt64().ToString());
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override JsonObjectStringWrapper ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            return new JsonObjectStringWrapper(value);
        }

        if (reader.TokenType == JsonTokenType.StartObject)
        {
            var json = JsonDocument.ParseValue(ref reader).RootElement;
            return new JsonObjectStringWrapper(json.ToString());
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return new JsonObjectStringWrapper(null);
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return new JsonObjectStringWrapper(reader.GetInt64().ToString());
        }

        throw new JsonException($"Unexpected token type: {reader.TokenType}");
    }

    public override void Write(Utf8JsonWriter writer, JsonObjectStringWrapper value, JsonSerializerOptions options)
    {
        if (value.Value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Value);
        }
    }
    public override void WriteAsPropertyName(Utf8JsonWriter writer, JsonObjectStringWrapper value, JsonSerializerOptions options)
    {
        if (value.Value == null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WritePropertyName(value.Value);
        }
    }
}

public class JsonObjectStringWrapper(string? value)
{
    public string? Value { get; set; } = value;
}