using System.Globalization;
using Newtonsoft.Json;

namespace AOSync.COMMON.Converters;

public class DateTimeConverter : JsonConverter
{
    private readonly string[] DateFormats = { "yyyy-MM-ddTHH:mm:ss.fffZ", "yyyy-MM-ddTHH:mm:ss.ffZ" };

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            if (objectType == typeof(DateTime?)) return null;

            throw new JsonSerializationException("Cannot convert null value to DateTime.");
        }

        if (reader.TokenType != JsonToken.String)
            throw new JsonSerializationException(
                $"Unexpected token parsing date. Expected String, got {reader.TokenType}.");

        var dateString = (string)reader.Value!;
        foreach (var format in DateFormats)
            if (DateTime.TryParseExact(dateString, format, null, DateTimeStyles.AdjustToUniversal, out var dateTime))
                return dateTime;

        throw new JsonSerializationException($"Unable to parse '{dateString}' as a valid DateTime.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (!(value is DateTime dateTime)) throw new JsonSerializationException("Expected date object value.");

        writer.WriteValue(dateTime.ToString(DateFormats[0])); // Choose one format for writing
    }
}