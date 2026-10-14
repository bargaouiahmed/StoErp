
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using StoErp.Modules.Emailing.Application.Ports;
using StoErp.Modules.Emailing.Infrastructure.Configurations;

namespace StoErp.Modules.Emailing.Infrastructure.Ports;

public class EmailPort : IEmailPort
{   
    private readonly SmtpOptions _smtpOptions;
    public EmailPort(SmtpOptions smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }
    
    public async Task SendEmailWithTemplateAsync(string to, string subject, string emailTemplate, CancellationToken cancellationToken = default)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Connect(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.Secure ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto, cancellationToken);
        smtpClient.Authenticate(_smtpOptions.Username, _smtpOptions.Password);
        
        var message = new MimeKit.MimeMessage();
        message.From.Add(new MailboxAddress("StoErp", _smtpOptions.Username));
        message.To.Add( new MailboxAddress(string.Empty,to));
        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = emailTemplate
        };

        await smtpClient.SendAsync(message, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);


    }
}
