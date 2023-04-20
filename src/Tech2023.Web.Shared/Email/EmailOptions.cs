using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Tech2023.Web.Shared.Email;

/// <summary>
/// Email settings used for the application
/// </summary>
public sealed class EmailOptions
{
    /// <summary>
    /// The email address that sends the email
    /// </summary>
    [EmailAddress]
    [Required]
    public string FromEmail { get; set; }

    /// <summary>
    /// The network port that the server connects to
    /// </summary>
    [Range(0, ushort.MaxValue)]
    [Required]
    public int Port { get; set; }

    /// <summary>
    /// The name of the sender
    /// </summary>
    [Required]
    public string SenderName { get; set; }

    /// <summary>
    /// The smtp server being used for the connection
    /// </summary>
    [Required]
    public string SmtpServer { get; set; }

    /// <summary>
    /// The smtp user name
    /// </summary>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// The password for smtp authentication
    /// </summary>
    [Required]
    public string Password { get; set; }
}
