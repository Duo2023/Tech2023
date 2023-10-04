using System.Diagnostics.CodeAnalysis;
using Tech2023.DAL;

namespace Tech2023.Web.Initialization.Json.Models;

internal class NceaStandardJsonModel
{
    public int AchievementStandard { get; set; }

    public NceaAssessmentType Type { get; set; }

#nullable disable
    public string Description { get; set; }
}
