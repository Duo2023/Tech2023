using System.Text.Json.Serialization;

using Tech2023.DAL.Models;

namespace Tech2023.DAL.SourceGenerators;

[JsonSerializable(typeof(PrivacyPolicy))]
public partial class PrivacyPolicyContext : JsonSerializerContext
{

}
