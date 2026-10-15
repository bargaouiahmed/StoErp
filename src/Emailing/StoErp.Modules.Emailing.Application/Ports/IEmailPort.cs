
using StoErp.Modules.Emailing.Application.Enums;

namespace StoErp.Modules.Emailing.Application.Ports;

public interface IEmailPort
{
    Task SendEmailWithTemplateAsync(string to, string subject, string emailTemplate, CancellationToken cancellationToken = default);

}
