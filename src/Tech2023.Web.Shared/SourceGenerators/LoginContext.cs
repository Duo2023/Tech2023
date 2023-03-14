using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared.SourceGenerators;

[JsonSerializable(typeof(Login))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class LoginContext : JsonSerializerContext
{

}
