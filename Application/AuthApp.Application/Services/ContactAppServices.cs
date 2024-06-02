using AuthApp.Domain.Email;
using AuthApp.Domain.Settings;
using AuthApp.Infra.CrossCutting.Resources;
using AuthApp.Services.Exceptions;
using AuthApp.Services.Utils.EmailSender;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace AuthApp.Application;

public class ContactAppServices : IContactAppServices
{
    #region Constants
    private const string CONTACT_FORM_DESCRIPTION = "PORTAL FORM";
    private const string CONTACT_FORM_BODY_FORMAT = "Nome: {0}<br/><br/>Assunto: {1}<br/><br/>Email: {2}<br/><br/>Telefone: {3}<br/><br/>Mensagem: {4}";
    #endregion

    private readonly IEmailSender _emailSender;
    public readonly EmailSettings _emailSettings;
    private readonly IValidator<ContactEmailRequest> _contactEmailRequestValidator;

    public ContactAppServices(IEmailSender emailSender, IOptions<EmailSettings> emailSettings, IValidator<ContactEmailRequest> contactEmailRequestValidator)
    {
        _emailSender = emailSender;
        _emailSettings = emailSettings.Value;
        _contactEmailRequestValidator = contactEmailRequestValidator;
    }

    public async Task SendEmailContactFormAsync(ContactEmailRequest request)
    {
        _contactEmailRequestValidator.ValidateAndThrow(request);

        var to = new List<To> { new To(CONTACT_FORM_DESCRIPTION, _emailSettings.From) };
        var body = string.Format(CONTACT_FORM_BODY_FORMAT, request.Name, request.Subject, request.Email, request.PhoneNumber, request.Message);
        var message = new Message(to, $"{CONTACT_FORM_DESCRIPTION}: {request.Subject}", body);

        var mimeMessage = _emailSender.CreateEmailMessage(message);

        try 
        {
            await _emailSender.SendAsync(mimeMessage);
        }
        catch (Exception e) 
        {
            throw new BusinessException(ErrorCodeResource.MAIL_ERROR_SENDING_EMAIL, e);
        }
    }
}

