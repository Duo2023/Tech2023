namespace Tech2023.Web.ViewModels;

/// <summary>
/// View model to support the rror page
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// The request id of the error
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Whether or not to show the <see cref="RequestId"/> (is it null or empty?)
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
