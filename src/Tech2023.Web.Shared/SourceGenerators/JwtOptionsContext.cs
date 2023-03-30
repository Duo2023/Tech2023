using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared.SourceGenerators;

[JsonSerializable(typeof(JwtOptions))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class JwtOptionsContext : JsonSerializerContext
{

}
