namespace AuthApp.Application;

public interface IContactAppServices
{
    /// <summary>
    /// Send email to your own message box
    /// </summary>
    /// <param name="request">Subject, email and message</param>
    Task SendEmailContactFormAsync(ContactEmailRequest request);
}
