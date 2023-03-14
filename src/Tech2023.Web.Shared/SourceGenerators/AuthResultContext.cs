using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared.SourceGenerators;

[JsonSerializable(typeof(AuthResult))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class AuthResultContext : JsonSerializerContext
{

}
