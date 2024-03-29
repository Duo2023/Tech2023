﻿namespace Tech2023.Web.Initialization;

/// <summary>
/// Initializes the data inside a DbContext
/// </summary>
public interface IDataInitializer
{
    /// <summary>
    /// Initializes the database
    /// </summary>
    Task InitializeAsync();
}
