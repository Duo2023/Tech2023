using System.Diagnostics;

using Microsoft.AspNetCore.Http;

namespace Tech2023.Web.Shared.Files;

/// <summary>
/// Implements file saving through the <see cref="IFileSaver"/> interface
/// </summary>
public sealed class FileSaver : IFileSaver
{
    // options for creating the file stream object
    internal static readonly FileStreamOptions _options = new()
    {
        Access = FileAccess.Write,
        Mode = FileMode.Create,
        Options = FileOptions.Asynchronous
    };

    /// <inheritdoc/>
    public async Task SaveAsync(string baseUrl, string name, IFormFile file, bool overrideFile = false, CancellationToken cancellationToken = default)
    {
        // the caller of this method should handle null values beforehand
        Debug.Assert(baseUrl != null);
        Debug.Assert(name != null);

        string destination = Path.Combine(baseUrl, name);

        // if the file already exists 
        if (!overrideFile && File.Exists(destination))
        {
            return;
        }

        FileStream? fileStream = null;

        try
        {
            fileStream = new FileStream(destination, _options);

            await file.CopyToAsync(fileStream, cancellationToken);
        }
        finally
        {
            fileStream?.Dispose();
        }
    }
}
