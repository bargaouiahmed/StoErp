
using StoErp.Modules.Emailing.Application.Enums;

namespace StoErp.Modules.Emailing.Application.Resolvers;

public interface IEmailTemplateResolver
{
    string Resolve(EmailTemplates emailTemplate, IReadOnlyDictionary<string, string> parameters);

}
