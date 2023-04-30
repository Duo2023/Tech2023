using System.Diagnostics.CodeAnalysis;

namespace Tech2023.Web.Shared;

/// <summary>
/// Marks a field is a route field
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class RouteVisibilityAttribute : Attribute
{
    /// <summary>
    /// The visiblity of the route attribute
    /// </summary>
    public Visiblity Visiblity { get; }

    /// <summary>
    /// Whether the route is relative to the <see cref="BaseRoute"/>
    /// </summary>
    public bool IsRelative => BaseRoute != null;

    /// <summary>
    /// Gets the base route if any, of the attribute
    /// </summary>
    [MemberNotNullWhen(true, nameof(IsRelative))]
    public string? BaseRoute { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteVisibilityAttribute"/> class
    /// </summary>
    public RouteVisibilityAttribute(Visiblity visiblity) 
    {
        Visiblity = visiblity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteVisibilityAttribute"/> class
    /// </summary>
    /// <param name="visibility">The visibility of the route</param>
    /// <param name="baseUrl">The base url of the route</param>
    public RouteVisibilityAttribute(Visiblity visibility, string baseUrl)
    {
        ArgumentException.ThrowIfNullOrEmpty(baseUrl);

        Visiblity = visibility;
        BaseRoute = baseUrl;
    }
}
