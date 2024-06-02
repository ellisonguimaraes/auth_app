using AuthApp.Domain.Email;
using MimeKit;

namespace AuthApp.Services.Utils.EmailSender;

public interface IEmailSender
{
    MimeMessage CreateEmailMessage(Message message);

    Task SendAsync(MimeMessage mailMessage);
}
