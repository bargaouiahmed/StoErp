using System.Text.RegularExpressions;
using StoErp.Modules.Emailing.Application.Enums;
using StoErp.Modules.Emailing.Application.Resolvers;

namespace StoErp.Modules.Emailing.Infrastructure.Resolvers;

public sealed partial class EmailTemplateResolver : IEmailTemplateResolver
{
    private const string TemplatesFolder = "Templates";

    public string Resolve(EmailTemplates emailTemplate, IReadOnlyDictionary<string, string> parameters)
    {
        var templateFileName = emailTemplate switch
        {
            EmailTemplates.VerificationEmailTemplateEn => "verification-template-en.html",
            EmailTemplates.VerificationEmailTemplateFr => "verification-template-fr.html",
            _ => throw new ArgumentOutOfRangeException(nameof(emailTemplate), emailTemplate, null)
        };

        var content = LoadTemplateFromFile(templateFileName);

        if (parameters.Count == 0)
        {
            return content;
        }

        return PlaceholderPattern().Replace(content, match =>
        {
            var key = match.Groups["key"].Value;
            return parameters.TryGetValue(key, out var replacement) ? replacement : match.Value;
        });
    }

    [GeneratedRegex(@"\{\{\s*(?<key>[\w.]+)\s*\}\}", RegexOptions.Compiled)]
    private static partial Regex PlaceholderPattern();

    private static string LoadTemplateFromFile(string templateFileName)
    {
        var templatePath = Path.Combine(AppContext.BaseDirectory, TemplatesFolder, templateFileName);
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Email template file '{templateFileName}' not found at path '{templatePath}'.", templatePath);
        }

        return File.ReadAllText(templatePath);
    }
}
