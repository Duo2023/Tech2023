using System.Text.Json.Serialization;
using Tech2023.Web.Initialization.Json.Models;

namespace Tech2023.Web.Initialization.Json;

// this file is a JSON source generator so we don't have to specify camel case for all of our objects and also for faster startup and serialization
// see https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/#why-do-source-generation

[JsonSerializable(typeof(SubjectJsonModel[]))]
[JsonSerializable(typeof(UserModel[]))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class SeedSerializationContext : JsonSerializerContext { };
