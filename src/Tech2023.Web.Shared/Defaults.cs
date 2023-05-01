using System.Net.Http.Headers;

namespace Tech2023.Web.Shared;

/// <summary>
/// The web defaults used by the API
/// </summary>
public static class Defaults
{
    /// <summary>
    /// The default content type used by the controller, this is JSON content
    /// </summary>
    public const string ContentType = "application/json";
    
    /// <summary>
    /// The default schema of the controller which is /api/name/
    /// </summary>
    public const string Controller = "api/[controller]";

    /// <summary>
    /// Default content header type
    /// </summary>
    public static readonly MediaTypeHeaderValue ContentTypeHeader = new(ContentType);
}
