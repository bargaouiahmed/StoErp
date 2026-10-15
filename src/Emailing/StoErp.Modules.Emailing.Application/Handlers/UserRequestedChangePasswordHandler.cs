

using System.ComponentModel.DataAnnotations;
using StoErp.Modules.Emailing.Application.Enums;
using StoErp.Modules.Emailing.Application.Ports;
using StoErp.Modules.Emailing.Application.Resolvers;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Emailing.Application.Handlers;

public class UserRequestedChangePasswordHandler : IEventHandler<UserRequestedChangePassword>
{
    private readonly IEmailPort _emailPort;
    private readonly IEmailTemplateResolver _emailTemplateProvider;

    public UserRequestedChangePasswordHandler(IEmailPort emailPort, IEmailTemplateResolver emailTemplateProvider)
    {
        _emailPort = emailPort;
        _emailTemplateProvider = emailTemplateProvider;
    }
    public async Task HandleAsync(UserRequestedChangePassword eventMessage, CancellationToken cancellationToken = default)
    {
        var emailTemplate = eventMessage.LanguagePreference switch
        {
            "en" => EmailTemplates.PasswordResetEmailTemplateEn,
            "fr" => EmailTemplates.PasswordResetEmailTemplateFr,
            _=>throw new ArgumentOutOfRangeException($"Unsupported language preference: {eventMessage.LanguagePreference}")
        };
        var emailTemplateHtml = _emailTemplateProvider.Resolve(emailTemplate,new Dictionary<string, string>
        {
            {"username", eventMessage.Username},
            {"PasswordResetToken", eventMessage.PasswordResetTokenSecret},
            {"Year", DateTimeOffset.UtcNow.Year.ToString()},
        });
        var subject = eventMessage.LanguagePreference switch
        {
            "en" => "Password Reset Request",
            "fr" => "Demande de réinitialisation du mot de passe",
            _=>throw new ArgumentOutOfRangeException($"Unsupported language preference: {eventMessage.LanguagePreference}")
        };
        await _emailPort.SendEmailWithTemplateAsync(eventMessage.Email, subject , emailTemplateHtml, cancellationToken);

    }
}
