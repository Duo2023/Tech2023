using System.Text.Json.Serialization;

using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// A view model of the subject that is just used to reference models
/// </summary>
public sealed class SubjectViewModel
{
    /// <summary>
    /// The identifier of the Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the subject
    /// </summary>
    [JsonPropertyName("name")]  
    public string Name { get; set; }

    /// <summary>
    /// The level of the subject
    /// </summary>
    [JsonPropertyName("level")]
    public CurriculumLevel Level { get; set; }

    /// <summary>
    /// The source of the subject
    /// </summary>
    [JsonPropertyName("source")]
    public CurriculumSource Source { get; set; }

    /// <summary>
    /// Unsigned 4 byte integer used for the preffered display color
    /// </summary>
    [JsonPropertyName("displayColor")]
    public uint DisplayColor { get; set; }

    /// <summary>
    /// Explicity casts a subject to a view model of a subject
    /// </summary>
    /// <param name="s"></param>
    public static explicit operator SubjectViewModel(Subject s)
    {
        return new()
        {
            Id = s.Id,
            Name = s.Name,
            Level = s.Level,
            Source = s.Source,
            DisplayColor = s.DisplayColor
        };
    }
}
