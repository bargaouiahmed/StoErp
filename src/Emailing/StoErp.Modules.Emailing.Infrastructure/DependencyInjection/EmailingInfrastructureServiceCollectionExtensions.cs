
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoErp.Modules.Emailing.Application.Handlers;
using StoErp.Modules.Emailing.Application.Ports;
using StoErp.Modules.Emailing.Application.Resolvers;
using StoErp.Modules.Emailing.Infrastructure.Configurations;
using StoErp.Modules.Emailing.Infrastructure.Ports;
using StoErp.Modules.Emailing.Infrastructure.Resolvers;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;

namespace StoErp.Modules.Emailing.Infrastructure.DependencyInjection;

public static class EmailingInfrastructureServiceCollectionExtensions 
{
    public static IServiceCollection AddEmailingInfrastructure(this IServiceCollection _services, IConfiguration _configuration)
    {
        _services.AddSingleton(new SmtpOptions
        {
            Host = _configuration["SmtpOptions:Host"] ?? throw new InvalidOperationException("SMTP Host not found in configuration."),
            Port = int.Parse(_configuration["SmtpOptions:Port"] ?? throw new InvalidOperationException("SMTP Port not found in configuration.")),
            Username = _configuration["SmtpOptions:User"] ?? throw new InvalidOperationException("SMTP Username not found in configuration."),
            Password = _configuration["SmtpOptions:Pass"] ?? throw new InvalidOperationException("SMTP Password not found in configuration."),
            Secure = bool.Parse(_configuration["SmtpOptions:Secure"] ?? throw new InvalidOperationException("SMTP Secure option not found in configuration."))
        });

        _services.AddScoped<IEmailPort, EmailPort>();
        _services.AddScoped<IEmailTemplateResolver, EmailTemplateResolver>();
        _services.AddScoped<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
        _services.AddScoped<IEventHandler<UserRequestedChangePassword>, UserRequestedChangePasswordHandler>();
        return _services;
    }
}
