using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Initialization;

// this file supports the Initialzer by loading objects from JSON and converting them into the desired object

[JsonSerializable(typeof(SubjectJsonModel[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class SeedSerializationContext : JsonSerializerContext { };

#nullable disable

// this type is a smaller version of Subject and only supports reads from JSON and not writes

internal class SubjectJsonModel
{
    public string Name { get; set; }

    [JsonConverter(typeof(LevelConverter))]
    public (CurriculumLevel Level, CurriculumSource Source) Curriculum { get; set; }

    public ResourceModel[] Resources { get; set; }

    public static explicit operator Subject(SubjectJsonModel model)
    {
        return new()
        {
            Name = model.Name.ToUpper(), // Make sure to keep this
            Source = model.Curriculum.Source,
            Level = model.Curriculum.Level,
            DisplayColor = (uint)Random.Shared.Next()
        };
    }
}

#nullable restore

internal class ResourceModel
{
    [JsonPropertyName("nceaStandard")]
    public NceaStandard? Standard { get; set; }

    [JsonPropertyName("cambridgeStandard")]
    [JsonConverter(typeof(CambridgeResourceConverter))]
    public CambridgeStandard? CambridgeStandard { get; set; }

    [JsonPropertyName("items")]
    public ItemModel? Items { get; set; }
}

internal class CambridgeStandard
{
    public int Number { get; set; }
    public Season Season { get; set; }
    public Variant Variant { get; set; }
}

internal class NceaStandard
{
    public int AchievementStandard { get; set; }
    public NceaAssessmentType Type { get; set; }
    public string? Description { get; set; }
}

internal class ItemModel
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("file")]
    public string? File { get; set; }
}

// converts level and curriculum in one converter

internal class LevelConverter : JsonConverter<(CurriculumLevel Level, CurriculumSource Source)>
{
    public override (CurriculumLevel Level, CurriculumSource Source) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (Curriculum.TryParse(str, out CurriculumLevel level, out CurriculumSource source))
        {
            return (level, source);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    [DoesNotReturn]
    public override void Write(Utf8JsonWriter writer, (CurriculumLevel Level, CurriculumSource Source) value, JsonSerializerOptions options)
    {
        throw new NotImplementedException("This type supports reading but not writing from");
    }
}

internal class CambridgeResourceConverter : JsonConverter<CambridgeStandard>
{
    public override CambridgeStandard Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

    public override void Write(Utf8JsonWriter writer, CambridgeStandard value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(Cambridge.GetString(value.Number, value.Season, value.Variant));
    }
}
