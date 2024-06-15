using AuthApp.Domain.Email;
using AuthApp.Domain.Settings;
using AuthApp.Infra.CrossCutting.Resources;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Diagnostics;

namespace AuthApp.Services.Utils.EmailSender;

public class EmailSender : IEmailSender
{
    private const string AUTHENTICATION_MECHANISMS_REMOVE = "XOAUTH2";

    private readonly EmailSettings _emailSettings;

    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<EmailSettings> emailSettings, ILogger<EmailSender> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    /// <summary>
    /// Send Email
    /// </summary>
    /// <param name="mailMessage">MimeMessage</param>
    public async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient
        {
            Timeout = 15000
        };

        try
        {
            await client.ConnectAsync(_emailSettings.Smtp, _emailSettings.Port, true);

            client.AuthenticationMechanisms.Remove(AUTHENTICATION_MECHANISMS_REMOVE);

            await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);

            await client.SendAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{ex}, {ErrorCodeResource.MAIL_ERROR_SENDING_EMAIL}, TraceId: {Activity.Current?.Id}");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    /// <summary>
    /// Create email message
    /// </summary>
    /// <param name="message">Message informations</param>
    /// <returns>MimeMessage</returns>
    public MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = message.Content };

        return emailMessage;
    }
}