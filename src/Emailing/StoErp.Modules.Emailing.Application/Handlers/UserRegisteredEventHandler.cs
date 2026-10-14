using System;
using StoErp.Modules.Emailing.Application.Enums;
using StoErp.Modules.Emailing.Application.Ports;
using StoErp.Modules.Emailing.Application.Resolvers;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Emailing.Application.Handlers;

public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
{

    private readonly IEmailPort _emailPort;
    private readonly IEmailTemplateResolver _emailTemplateResolver;
    public UserRegisteredEventHandler(IEmailPort emailPort, IEmailTemplateResolver emailTemplateResolver)
    {
        _emailPort = emailPort;
        _emailTemplateResolver = emailTemplateResolver;

    }
    public async Task HandleAsync(UserRegisteredEvent eventMessage, CancellationToken cancellationToken = default)
    {
        var isFrench = eventMessage.UserLanguagePreference.StartsWith("fr", StringComparison.OrdinalIgnoreCase);

        var (template, subject) = isFrench
            ? (EmailTemplates.VerificationEmailTemplateFr, "Bienvenue sur StoErp : vérifiez votre compte")
            : (EmailTemplates.VerificationEmailTemplateEn, "Welcome to StoErp: verify your account");

        var parameters = new Dictionary<string, string>
        {
            ["username"] = eventMessage.Username,
            ["verificationCode"] = eventMessage.VerificationTokenSecret,
            ["currentYear"] = DateTime.UtcNow.Year.ToString()
        };

        var body = _emailTemplateResolver.Resolve(template, parameters);

        await _emailPort.SendEmailWithTemplateAsync(eventMessage.Email, subject, body, cancellationToken);
    }
}
