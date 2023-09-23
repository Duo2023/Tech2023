namespace Tech2023.Web.Models;

#nullable disable

/// <summary>
/// Subject list model, which contains a view only portion of the subjects and user subjects
/// </summary>
public class SubjectListModel
{
    public List<SubjectViewModel> NceaSubjects { get; set; }

    public List<SubjectViewModel> CambridgeSubjects { get; set; }

    public List<SubjectViewModel> ExistingNceaSubjects { get; set; }

    public List<SubjectViewModel> ExistingCambridgeSubjects { get; set; }
}
