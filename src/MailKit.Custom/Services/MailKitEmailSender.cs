﻿using MailKit.Custom.InputModels;
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

            //Connessione al server di posta, indicando HOST, PORT e SECURITY
            await client.ConnectAsync(options.Host, options.Port, options.Security);

            //Server di posta richiede l'autenticazione tramite username e password
            if (!string.IsNullOrEmpty(options.Username))
            {
                await client.AuthenticateAsync(options.Username, options.Password);
            }

            MimeMessage message = new();

            //Imposto il formato del mittente
            if (!string.IsNullOrEmpty(model.mailExtender.MittenteNominativo))
            {
                message.From.Add(MailboxAddress.Parse($"{model.mailExtender.MittenteNominativo} <{model.mailExtender.MittenteEmail}>"));
            }
            else
            {
                message.From.Add(MailboxAddress.Parse(model.Mittente));
            }

            //Imposto il formato del destinatario
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

            //Allegato/i
            var allegato = model.mailExtender.Allegato;
            var allegati = model.mailExtender.Allegati;

            if (allegati.Count > 0)
            {
                if (allegati != null)
                {
                    byte[] fileBytes;

                    foreach (var file in allegati)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }

                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
            }
            else
            {
                if (allegato != null)
                {
                    byte[] fileBytes;

                    if (allegato.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            allegato.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(allegato.FileName, fileBytes, ContentType.Parse(allegato.ContentType));
                    }
                }
            }

            if (!model.FormatoHtml)
            {
                //Formato TESTO
                builder.TextBody = model.Messaggio;
            }
            else
            {
                //Formato HTML
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