using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// Represents an NCEA resource, this is used for external standards and internal standards as well as unit standards
/// </summary>
public sealed class NceaResource
{
    /// <summary>
    /// The identifier of the NCEA resource, this value is not related to the resource at all and it just provided a unique identifier to use in the database
    /// that can be created at random with a 2^128 amount of values to be generated from for uniqueness
    /// </summary>
    [Key]
    [JsonPropertyName("id")] //
    public Guid Id { get; set; }

    /// <summary>
    /// The achievement standard number for the NCEA resource, there are different year level revisions for this however
    /// </summary>
    [Required]
    [JsonPropertyName("achievementStandard")]
    public int AchievementStandard { get; set; }

    /// <summary>
    /// The revision year for the assesment, this is what differs the version between papers because 
    /// </summary>
    [Required]
    [JsonPropertyName("revisionYear")]
    public int RevisionYear { get; set; }

    /// <summary>
    /// The type of assement the resource is related to
    /// </summary>
    [Required]
    [JsonPropertyName("assessmentType")]
    public NceaAssessmentType AssessmentType { get; set; }

    /// <summary>
    /// The last time the NCEA resource was updated in the database, 
    /// this is not the same time it may have been updated on the ncea website
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }
}
