using Microsoft.AspNetCore.Http;

namespace Tech2023.Web.Shared.Files;

/// <summary>
/// Abstract used for saving files to a location
/// </summary>
public interface IFileSaver
{
    /// <summary>
    /// Saves a file to a specified location combining the base url and the name of the file, using the data contained in a IFormFile
    /// </summary>
    /// <param name="baseUrl">The base url location to save to</param>
    /// <param name="name">The name of the file</param>
    /// <param name="file">The file data itself</param>
    /// <param name="overrideFile">Whether to override an existing file if it already exists at a location</param>
    /// <param name="cancellationToken">A cancellation token to cancel the method if needed</param>
    Task SaveAsync(string baseUrl, string name, IFormFile file, bool overrideFile = false, CancellationToken cancellationToken = default);
}
