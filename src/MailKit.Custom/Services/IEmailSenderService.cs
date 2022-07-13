using MailKit.Custom.InputModels;

namespace MailKit.Custom.Services;
public interface IEmailSenderService
{
    Task SendEmailAsync(InputMailSender model);
}
