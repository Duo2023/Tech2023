namespace Tech2023.Web.ViewModels;

#nullable disable

/// <summary>
/// A model used to support the search function
/// </summary>
public class SearchResults
{
    /// <summary>
    /// The query supplied for the search
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    /// A list of search results, if any
    /// </summary>
    public List<SearchResult> Results { get; set; }
}
