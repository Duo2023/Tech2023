using System.Text.Json.Serialization;

namespace Tech2023.Web.Workers;

/// <summary>
/// The options used for the background paper crawler service, this is used to configure the settings of it
/// </summary>
public class PaperCrawlerOptions
{
    /// <summary>
    /// Whether the crawler is enabled for runs or not
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; }

    /// <summary>
    /// The frequency that the web crawler runs, this property is measured in minutes
    /// </summary>
    [JsonPropertyName("frequency")]
    public int Frequency { get; set; }
}
