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
