# MailKit Custom

[![NuGet](https://img.shields.io/nuget/v/MailKit.Custom.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/MailKit.Custom)
[![Nuget](https://img.shields.io/nuget/dt/MailKit.Custom.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/MailKit.Custom)
[![MIT](https://img.shields.io/github/license/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)](https://github.com/AngeloDotNet/MailKit.Custom/blob/master/LICENSE)
![Github](https://img.shields.io/github/last-commit/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)
[![Github](https://img.shields.io/github/contributors/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)](https://github.com/AngeloDotNet/MailKit.Custom/graphs/contributors)


### Installation

The library is available on [NuGet](https://www.nuget.org/packages/MailKit.Custom).


### Usage/Examples

An example of how to invoke the SendEmailAsync method


**NOTE:** It is possible to use both for the sender and for the recipient the short format (by evaluating the MITTENTE and DESTINATARIO fields) or the extended format (by evaluating the fields MITTENTENOMINATIVO, MITTENTEEMAIL, DESTINATARIONOMINATIVO and DESTINATARIOEMAIL).

*Do NOT USE both formats, but choose only one type.*


*Model example*
```
namespace MyProject;

public class InputMail
{
    public string Mittente { get; set; }
    public string Destinatario { get; set; }
    public string Oggetto { get; set; }               //Required field
    public string Messaggio { get; set; }             //Required field
    public bool FormatoHtml { get; set; }             //Set TRUE if you want to send emails in HTML format
    public InputOptions mailOptions { get; set; }
    public InputExtender mailExtender { get; set; }  
}

public partial class InputOptions
{
    public string Host { get; set; }                  //Required field
    public int Port { get; set; }                     //Required field
    public SecureSocketOptions Security { get; set; } //Required field
    public string Username { get; set; }              //Mandatory field if the mailserver requires authentication
    public string Password { get; set; }              //Mandatory field if the mailserver requires authentication
}

public partial class InputExtender
{
    public string MittenteNominativo { get; set; }
    public string MittenteEmail { get; set; }
    public string DestinatarioNominativo { get; set; }
    public string DestinatarioEmail { get; set; }
}
```


*Class example*
```
namespace MyProject;

public class MyClass
{
  private readonly IEmailSenderService emailService;

  public MyClass(IEmailSenderService emailService)
  {
    this.emailService = emailService;
  }

  public async Task<IActionResult> InvioEmail([FromForm] InputMail model)
  {
      try
      {
          await emailService.SendEmailAsync(model);
          return Ok();
      }
      catch
      {
          throw new Exception();
      }
  }
}
```
