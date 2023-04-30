namespace Tech2023.Web.API;

/// <summary>
/// Initializes the data inside a DbContext
/// </summary>
public interface IDataInitializer
{
    Task InitializeAsync();
}
