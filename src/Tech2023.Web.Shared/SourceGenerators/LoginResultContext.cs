using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared;

[JsonSerializable(typeof(LoginResult))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class LoginResultContext : JsonSerializerContext
{

}
