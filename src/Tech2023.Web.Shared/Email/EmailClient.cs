using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Tech2023.Web.Shared.Email;

/// <summary>
/// The default email client of the application which uses an SMTP server to send the mail
/// </summary>
public class EmailClient : IEmailClient
{
    internal readonly ILogger<IEmailClient> _logger;
    internal readonly IOptions<EmailOptions> _options;

    /// <summary>
    /// Initializes a new instance 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public EmailClient(IOptions<EmailOptions> options, ILogger<IEmailClient> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNull(email);

        await ExceuteAsync(email, subject, htmlMessage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal async Task ExceuteAsync(string targetAddress, string subject, string htmlMessage)
    {
        var email = new MimeMessage()
        {
            Sender = MailboxAddress.Parse(_options.Value.FromEmail),
            Subject = subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage }
        };

        email.Sender.Name = _options.Value.SenderName;

        email.From.Add(email.Sender);

        email.To.Add(MailboxAddress.Parse(targetAddress));

        using var client = new SmtpClient();

        // TODO: Consider adding error handling methods, where if the email fails to send instead of ignoring the error,
        // requeue the message on to a background service where emails can be retried.

        await client.ConnectAsync(_options.Value.SmtpServer, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);

        await client.AuthenticateAsync(_options.Value.Username, _options.Value.Password).ConfigureAwait(false);

        var response = await client.SendAsync(email).ConfigureAwait(false);

        _logger.LogInformation("{emailSendResponse}", response);
    }
}
