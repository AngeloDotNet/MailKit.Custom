# MailKit Custom

[![NuGet](https://img.shields.io/nuget/v/MailKit.Custom.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/MailKit.Custom)
[![Nuget](https://img.shields.io/nuget/dt/MailKit.Custom.svg?logo=nuget&style=for-the-badge)](https://www.nuget.org/packages/MailKit.Custom)
[![MIT](https://img.shields.io/github/license/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)](https://github.com/AngeloDotNet/MailKit.Custom/blob/master/LICENSE)
![Github](https://img.shields.io/github/last-commit/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)
[![Github](https://img.shields.io/github/contributors/AngeloDotNet/MailKit.Custom?logo=github&style=for-the-badge)](https://github.com/AngeloDotNet/MailKit.Custom/graphs/contributors)


## Installation

The library is available on [NuGet](https://www.nuget.org/packages/MailKit.Custom).


## How to usage

An example of how to invoke the SendEmailAsync method

**NOTE:** It is possible to use both for the sender and for the recipient the short format (by evaluating the *MITTENTE* and *DESTINATARIO* fields).
Or the extended format (by evaluating the fields *MITTENTENOMINATIVO*, *MITTENTEEMAIL*, *DESTINATARIONOMINATIVO* and *DESTINATARIOEMAIL*).

*Do NOT USE both formats, but choose only one type.*


## Example of use

*Email Controller*
```
using System;
using System.Threading.Tasks;
using MailKit.Custom.InputModels;
using MailKit.Custom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> logger;
        private readonly IEmailSenderService emailService;

        public EmailController(ILogger<EmailController> logger, IEmailSenderService emailService)
        {
            this.logger = logger;
            this.emailService = emailService;
        }

        [HttpPost("InvioEmail")]
        public async Task<IActionResult> InvioEmail([FromForm] InputMailSender model)
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
}
```

*Method CONFIGURE class STARTUP
```
public void ConfigureServices(IServiceCollection services)
{
    //OMISSIS

    services.AddTransient<IEmailSenderService, MailKitEmailSender>();

    //OMISSIS
}
```