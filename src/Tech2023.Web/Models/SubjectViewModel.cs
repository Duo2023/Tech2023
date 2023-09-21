using System.Text.Json.Serialization;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Models;

#nullable disable

/// <summary>
/// A view model of the subject that is just used to reference models
/// </summary>
public class SubjectViewModel
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]  
    public string Name { get; set; }

    [JsonPropertyName("level")]
    public CurriculumLevel Level { get; set; }
}
