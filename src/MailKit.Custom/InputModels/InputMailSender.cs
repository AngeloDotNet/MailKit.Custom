using System.ComponentModel.DataAnnotations;
using MailKit.Security;

namespace MailKit.Custom.InputModels;

public class InputMailSender
{
    public string Mittente { get; set; }
    public string Destinatario { get; set; }

    [Required]
    public string Oggetto { get; set; }

    [Required]
    public string Messaggio { get; set; }

    public bool FormatoHtml { get; set; }
    public InputMailOptions mailOptions { get; set; }
    public InputMailExtender mailExtender { get; set; }
}

public partial class InputMailOptions
{
    [Required]
    public string Host { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public SecureSocketOptions Security { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
}

public partial class InputMailExtender
{
    public string MittenteNominativo { get; set; }
    public string MittenteEmail { get; set; }
    public string DestinatarioNominativo { get; set; }
    public string DestinatarioEmail { get; set; }
}