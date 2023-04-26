namespace Tech2023.Web.Shared;

/// <summary>
/// Marks a field is a Public Route and can be routed to, regardless of authentication state of the user
/// </summary>
[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class PublicRouteAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PublicRouteAttribute"/> class
    /// </summary>
    public PublicRouteAttribute() 
    {
        // nop
        // ret
    }
}
