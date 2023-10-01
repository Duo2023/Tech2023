using Tech2023.DAL.Models;

namespace Tech2023.Web.ViewModels;

public sealed class BrowsePapersViewModel
{
    public required Subject SelectedSubject { get; set; }

    public string SelectedCategory { get; } = "Year";

    public List<string> Categories { get; set; } = new();

    public List<List<string>> CategoryValues { get; set; } = new();

    public List<List<Item>> CategorizedPapers { get; set; } = new();

    public string? FilterArgs { get; set; }

    public Item? SelectedPaper { get; set; }
}
