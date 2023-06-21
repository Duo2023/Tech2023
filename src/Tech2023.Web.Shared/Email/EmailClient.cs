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
    internal readonly MailboxAddress _senderAddress;

    /// <summary>
    /// Initializes a new instance 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    public EmailClient(IOptions<EmailOptions> options, ILogger<IEmailClient> logger)
    {
        _options = options;
        _logger = logger;
        _senderAddress = MailboxAddress.Parse(options.Value.FromEmail);
    }

    /// <inheritdoc/>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNull(email);

        var result = await ExceuteAsync(email, subject, htmlMessage);

        /*
         * TODD: Implement email retry sevice
         * 
         * Possible ideas, immediate retry, con of resource intensive,
         * Queue to background queue, too long a wait time??
         */
        if (!result) 
        {
            _logger.LogError("Email failed to send to: {address}", email);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal async Task<bool> ExceuteAsync(string targetAddress, string subject, string htmlMessage)
    {
        var email = new MimeMessage()
        {
            Sender = _senderAddress,
            Subject = subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage }
        };

        email.Sender.Name = _options.Value.SenderName;

        email.From.Add(email.Sender);

        if (!MailboxAddress.TryParse(targetAddress, out MailboxAddress target))
        {
            return false;
        }

        email.To.Add(target);

        var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_options.Value.SmtpServer, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);

            await client.AuthenticateAsync(_options.Value.Username, _options.Value.Password).ConfigureAwait(false);

            var res = await client.SendAsync(email).ConfigureAwait(false);

            _logger.LogInformation("{emailResponse}", res);
        }
        catch (Exception ex)
        {
            _logger.LogError("{exception}", ex);
            return false;
        }
        finally
        {
            client.Dispose();
        }

        return true;
    }
}
