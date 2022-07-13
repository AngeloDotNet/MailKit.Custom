using MailKit.Custom.InputModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MailKit.Custom.Services;
public class MailKitEmailSender : IEmailSenderService
{
    private readonly ILogger<MailKitEmailSender> logger;

    public MailKitEmailSender(ILogger<MailKitEmailSender> logger)
    {
        this.logger = logger;
    }

    public async Task SendEmailAsync(InputMailSender model)
    {
        try
        {
            var options = model.mailOptions;

            using SmtpClient client = new();

            await client.ConnectAsync(options.Host, options.Port, options.Security);

            //Server di posta richiede l'autenticazione tramite username e password
            if (!string.IsNullOrEmpty(options.Username))
            {
                await client.AuthenticateAsync(options.Username, options.Password);
            }

            MimeMessage message = new();

            //Mittente
            if (!string.IsNullOrEmpty(model.mailExtender.MittenteNominativo))
            {
                message.From.Add(MailboxAddress.Parse($"{model.mailExtender.MittenteNominativo} <{model.mailExtender.MittenteEmail}>"));
            }
            else
            {
                message.From.Add(MailboxAddress.Parse(model.Mittente));
            }

            //Destinatario
            if (!string.IsNullOrEmpty(model.mailExtender.DestinatarioNominativo))
            {
                message.To.Add(MailboxAddress.Parse($"{model.mailExtender.DestinatarioNominativo} <{model.mailExtender.DestinatarioEmail}>"));
            }
            else
            {
                message.From.Add(MailboxAddress.Parse(model.Destinatario));
            }

            message.Subject = model.Oggetto;

            var builder = new BodyBuilder();

            if (!model.FormatoHtml)
            {
                //Messaggio formato TESTO
                builder.TextBody = model.Messaggio;
            }
            else
            {
                //Messaggio formato HTML
                builder.HtmlBody = model.Messaggio;
            }

            message.Body = builder.ToMessageBody();

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}