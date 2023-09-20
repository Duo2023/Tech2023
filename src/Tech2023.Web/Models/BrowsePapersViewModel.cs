using Tech2023.DAL.Models;

namespace Tech2023.Web.Models;

public sealed class BrowsePapersViewModel
{
    public required Subject SelectedSubject { get; set; }

    public string SelectedCategory { get; } = "Year";

    public Dictionary<string, List<Resource>> CategorizedPapers { get; set; } = new();

    public string? FilterArgs { get; set; }

    public Resource? SelectedPaper {get; set; }
}
