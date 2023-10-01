using Microsoft.AspNetCore.Identity;

namespace Tech2023.DAL.Identity;

/// <summary>
/// Represents a role in the application, this dictates
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationRole"/> class
    /// </summary>
    /// <param name="name">The name of the role</param>
    public ApplicationRole(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }
}
