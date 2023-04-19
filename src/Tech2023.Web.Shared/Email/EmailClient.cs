using MailKit.Net.Smtp;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

namespace Tech2023.Web.Shared.Email;

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

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNull(email);

        await ExceuteAsync(email, subject, htmlMessage);
    }

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

        await client.ConnectAsync(_options.Value.SmtpServer, _options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls).ConfigureAwait(false);

        await client.AuthenticateAsync(_options.Value.UserName, _options.Value.Password).ConfigureAwait(false);

        var response = await client.SendAsync(email).ConfigureAwait(false);

    }
}
