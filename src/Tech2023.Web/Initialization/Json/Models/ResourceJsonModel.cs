using System.Text.Json.Serialization;
using Tech2023.Web.Initialization.Json.Converters;

namespace Tech2023.Web.Initialization.Json.Models;

internal class ResourceModel
{
    [JsonPropertyName("nceaStandard")]
    public NceaStandardJsonModel? Standard { get; set; }

    [JsonPropertyName("cambridgeStandard")]
    [JsonConverter(typeof(CambridgeResourceConverter))]
    public CambridgeStandardJsonModel? CambridgeStandard { get; set; }

    [JsonPropertyName("items")]
    public ItemJsonModel? Items { get; set; }
}
