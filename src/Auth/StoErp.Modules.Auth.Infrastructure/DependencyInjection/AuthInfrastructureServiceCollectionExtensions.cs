using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoErp.Modules.Auth.Application.Commands.Login;
using StoErp.Modules.Auth.Application.Commands.RegisterUser;
using StoErp.Modules.Auth.Application.Ports.Outbox;
using StoErp.Modules.Auth.Application.Ports.Persistence;
using StoErp.Modules.Auth.Application.Ports.Security;
using StoErp.Modules.Auth.Infrastructure.DependencyInjection.Configurations;
using StoErp.Modules.Auth.Infrastructure.Persistence;
using StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Reader;
using StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Repositories;
using StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;
using StoErp.Modules.Auth.Infrastructure.Persistence.Repositories;
using StoErp.Modules.Auth.Infrastructure.Persistence.UnitOfWork;
using StoErp.Modules.Auth.Infrastructure.Security;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Infrastructure.Messaging.EventBus;

namespace StoErp.Modules.Auth.Infrastructure.DependencyInjection;

public static class AuthInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();
        });
        services.AddSingleton(new JwtOptions
        {
            Issuer = configuration["JwtOptions:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found in configuration."),
            Audience = configuration["JwtOptions:Audience"] ?? throw new InvalidOperationException("JWT Audience not found in configuration."),
            Secret = configuration["JwtOptions:Secret"] ?? throw new InvalidOperationException("JWT Secret not found in configuration.")
        });
        services.AddSingleton(new VerificationTokenOptions
        {
            ValidityPeriod = TimeSpan.FromMinutes(double.Parse(configuration["VerificationTokenOptions:VerificationTokenLifeTimeInMinutes"] ?? throw new InvalidOperationException("VerificationTokenLifeTimeInMinutes not found in configuration."))),
            Length = int.Parse(configuration["VerificationTokenOptions:VerificationTokenLength"] ?? throw new InvalidOperationException("VerificationTokenLength not found in configuration.")),
            OnlyNumbers = bool.Parse(configuration["VerificationTokenOptions:VerificationTokenOnlyNumbers"] ?? throw new InvalidOperationException("VerificationTokenOnlyNumbers not found in configuration."))
        });

        services.AddSingleton(new TokenLifeTimeOptions(
            TimeSpan.FromMinutes(double.Parse(configuration["TokenLifeTimeOptions:AccessTokenLifeTimeInMinutes"] ?? throw new InvalidOperationException("AccessTokenLifeTimeInMinutes not found in configuration."))),
            TimeSpan.FromDays(double.Parse(configuration["TokenLifeTimeOptions:RefreshTokenLifeTimeInDays"] ?? throw new InvalidOperationException("RefreshTokenLifeTimeInDays not found in configuration.")))
        ));
        services.AddScoped<IUnitOfWork, AuthUnitOfWork>();
        services.AddScoped<RegisterUserCommandHandler>();
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthOutboxWriter, AuthOutboxWriter>();
        services.AddScoped<IOutboxReader, AuthOutboxReader>();
        services.AddScoped<IAuthOutboxRepository, AuthOutboxRepository>();
        services.AddScoped<IEventPublisher, InProcessEventPublisher>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
        services.AddScoped<IRefreshTokenWriteRepository, RefreshTokenWriteRepository>();
        services.AddScoped<IRefreshTokenHasher, RefreshTokenHasher>();
        services.AddScoped<IVerificationTokenGenerator, VerificationTokenGenerator>();
        services.AddScoped<IVerificationTokenHasher, VerificationTokenHasher>();
        services.AddScoped<IVerificationTokenWriteRepository, VerificationTokenWriteRepository>();
        services.AddScoped<IVerificationTokenReadRepository, VerificationTokenReadRepository>();
        
        services.AddOutboxWriterStrategies(typeof(AuthOutboxWriter).Assembly);
        services.AddOutboxReaderStrategies(typeof(AuthOutboxReader).Assembly);
        return services;

    }

}
