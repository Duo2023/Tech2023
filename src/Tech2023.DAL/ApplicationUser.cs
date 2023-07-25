﻿using Microsoft.AspNetCore.Identity;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// Represents a user in the application, this used with <see cref="ApplicationDbContext"/> and during account creations.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public DateTimeOffset Created { get; set; }

    public DateTimeOffset Updated { get; set; }

#nullable disable
    public virtual ICollection<Subject> SavedSubjects { get; } = new List<Subject>();
}
