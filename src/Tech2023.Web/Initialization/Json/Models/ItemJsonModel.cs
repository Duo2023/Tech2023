using System.Text.Json.Serialization;

namespace Tech2023.Web.Initialization.Json.Models;

internal class ItemJsonModel
{
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("file")]
    public string? File { get; set; }
}
