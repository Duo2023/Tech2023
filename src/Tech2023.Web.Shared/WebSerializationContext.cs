﻿using System.Text.Json.Serialization;

using Tech2023.DAL.Models;
using Tech2023.Web.Shared.Authentication;

namespace Tech2023.Web.Shared;

// For each type to be used in source generation include as an attribute.

/// <summary>
/// JSON source generator for all web related contexts
/// </summary>
[JsonSerializable(typeof(PrivacyPolicy))] // privacy
[JsonSerializable(typeof(Login))] // auth
[JsonSerializable(typeof(Register))]
[JsonSerializable(typeof(EmailAddress))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
public partial class WebSerializationContext : JsonSerializerContext
{

}
