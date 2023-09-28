using System.ComponentModel.DataAnnotations;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// Represents an NCEA resource, this is used for external standards and internal standards as well as unit standards
/// </summary>
public sealed class NceaResource : CustomResource
{
    /// <summary>
    /// The type of NCEA assement that the resource is
    /// </summary>
    [Required]
    public NceaAssessmentType AssessmentType { get; set; }

    /// <summary>
    /// The achievement standard of the NCEA resource
    /// </summary>
    [Required]
    public int AchievementStandard { get; set; }

#nullable disable
    /// <summary>
    /// A description of the standard, usually one line
    /// </summary>
    [Required]
    public string Description { get; set; }

    public string DisplayStandard() => $"AS{AchievementStandard}";
}
