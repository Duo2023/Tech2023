using Microsoft.AspNetCore.Http;

namespace Tech2023.Web.Shared.Files;

/// <summary>
/// Abstract used for saving files to a location
/// </summary>
public interface IFileSaver
{
    Task SaveAsync(string baseUrl, string name, IFormFile file, bool overrideFile = false, CancellationToken cancellationToken = default);
}
