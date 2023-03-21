using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Statistics;

namespace Tech2023.Web.Shared.SourceGenerators;

[JsonSerializable(typeof(PingResponse))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class PingResponseContext
{

}
