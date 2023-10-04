using System.Text.Json.Serialization;
using System.Text.Json;
using Tech2023.DAL;
using Tech2023.Web.Initialization.Json.Models;

namespace Tech2023.Web.Initialization.Json.Converters;

internal class CambridgeResourceConverter : JsonConverter<CambridgeStandardJsonModel>
{
    public override CambridgeStandardJsonModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (Cambridge.TryParseResource(reader.GetString(), out var number, out var season, out var variant))
        {
            return new()
            {
                Number = number,
                Season = season,
                Variant = variant
            };
        }

        throw new JsonException("Could not parse resource");
    }

    public override void Write(Utf8JsonWriter writer, CambridgeStandardJsonModel value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Cambridge.GetString(value.Number, value.Season, value.Variant));
    }
}
