﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Tech2023.DAL.Models;

namespace Tech2023.DAL;

/// <summary>
/// The abstraction of the database itself, it contains all the tables and roles
/// </summary>
public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options
    /// </summary>
    /// <param name="options">Initializes a new instance of the <see cref="ApplicationDbContext"/></param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated(); // ensure that the database is actually created
    }

    /// <summary>
    /// Overriden method to add custom logic the the model builder
    /// </summary>
    /// <param name="builder">The builder to apply additional logic to</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }

#nullable disable
    /// <summary>
    /// Table for the privacy policies of our application
    /// </summary>
    public DbSet<PrivacyPolicy> PrivacyPolicies { get; set; }
}
