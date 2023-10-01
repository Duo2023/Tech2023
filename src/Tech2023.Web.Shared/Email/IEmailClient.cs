namespace Tech2023.Web.Shared.Email;

/// <summary>
/// Interface for an SMTP email client
/// </summary>
public interface IEmailClient
{
    /// <summary>
    /// Sends an email to the specified address
    /// </summary>
    /// <param name="email">The target email address</param>
    /// <param name="subject">The subject of the email</param>
    /// <param name="htmlMessage">The html message of the email</param>
    /// <exception cref="ArgumentNullException">If the email is null</exception>
    Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
}
