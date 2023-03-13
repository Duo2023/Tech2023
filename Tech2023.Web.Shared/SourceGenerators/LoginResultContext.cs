using System.Text.Json.Serialization;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared.SourceGenerators;

[JsonSerializable(typeof(LoginResult))]
internal partial class LoginResultContext : JsonSerializerContext
{

}
