using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Services;

public class DateTimeNullableConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            if (DateTime.TryParse(stringValue, out var result))
                return result;
        }

        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to nullable DateTime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString("O")); // ISO 8601 format
        else
            writer.WriteNullValue();
    }
}
