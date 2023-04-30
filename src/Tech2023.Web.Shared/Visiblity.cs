namespace Tech2023.Web.Shared;

/// <summary>
/// The visibility of the application routes, this is used for tests and other things
/// </summary>
public enum Visiblity : byte
{
    /// <summary>A public route that can be called by anyone</summary>
    Public,

    /// <summary>An authenticated route, where they have be signed in</summary>
    Authenticated,

    /// <summary>Only an administrator can call these routes</summary>
    Adminstrator
}
