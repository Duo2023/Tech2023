using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// Used for displaying an NCEA assessment
/// </summary>
public class NceaAssessmentViewModel : AssessmentViewModel<NceaResource> { };

/// <summary>
/// Used for displaying a Cambridge assessment
/// </summary>
public class CambridgeAssessmentViewModel : AssessmentViewModel<CambridgeResource> { };

/// <summary>
/// Serves as a base class for an assessment view model implementation
/// </summary>
/// <typeparam name="TResource">The assessment resource to use</typeparam>
public abstract class AssessmentViewModel<TResource> where TResource : CustomResource
{
    public SubjectViewModel Subject { get; set; }

    public TResource Resource { get; set; }
}
