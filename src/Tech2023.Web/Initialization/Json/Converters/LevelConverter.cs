using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Text.Json;
using Tech2023.DAL;

namespace Tech2023.Web.Initialization.Json.Converters;

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
