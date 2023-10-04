using System.Text.Json.Serialization;
using Tech2023.DAL.Models;
using Tech2023.DAL;
using Tech2023.Web.Initialization.Json.Converters;

namespace Tech2023.Web.Initialization.Json.Models;

#nullable disable

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
