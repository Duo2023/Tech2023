using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tech2023.Core.Json.Converters;

/// <summary>
/// Converts a <see cref="TimeSpan"/> from JSON and to JSON
/// </summary>
public sealed class TimeSpanConverter : JsonConverter<TimeSpan>
{
    // nop
    // ret

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSpanConverter"/> class
    /// </summary>
    public TimeSpanConverter() { /* empty constructor is required here */ }

    /// <summary>
    /// Reads a <see cref="TimeSpan"/> from a JSON property field as an long integer
    /// </summary>
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long ticks = reader.GetInt64();

        return TimeSpan.FromTicks(ticks);
    }

    /// <summary>
    /// Writes a <see cref="TimeSpan"/> to a JSON property field using the underlying long value
    /// </summary>
    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Ticks);
    }
}
