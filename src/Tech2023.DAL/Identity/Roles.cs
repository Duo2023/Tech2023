namespace Tech2023.DAL.Identity;

/// <summary>
/// Static class which holds all the roles of the application that can exist
/// </summary>
public static class Roles
{
    /// <summary>The adminstrative role</summary>
    public const string Administrator = nameof(Administrator);

    /// <summary>
    /// All the roles defined in the <see cref="Roles"/> class
    /// </summary>
    /// <returns>An enumerable to iterate over</returns>
    public static IEnumerable<string> All()
    {
        yield return Administrator;
    }
}
