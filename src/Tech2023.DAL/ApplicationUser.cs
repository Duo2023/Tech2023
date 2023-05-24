using Microsoft.AspNetCore.Identity;

namespace Tech2023.DAL;

/// <summary>
/// Represents a user in the application, this used with <see cref="ApplicationDbContext"/> and during account creations.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Updated { get; set; }
}
