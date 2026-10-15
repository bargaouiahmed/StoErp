using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StoErp.Modules.ProfileModule.Application.Handlers;
using StoErp.Modules.ProfileModule.Application.Persistence;
using StoErp.Modules.ProfileModule.Application.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Application.Ports;
using StoErp.Modules.ProfileModule.Infrastructure.Persistence;
using StoErp.Modules.ProfileModule.Infrastructure.Persistence.Repositories;
using StoErp.Modules.ProfileModule.Infrastructure.Persistence.UnitOfWork;
using StoErp.Shared.Application.Events;
using StoErp.Shared.Contracts.AuthModuleContracts;
using StoErp.Shared.Contracts.ProfileModuleContracts;

namespace StoErp.Modules.ProfileModule.Infrastructure.DependencyInjection;

public static class ProfileInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddProfileInfrastructure(this IServiceCollection _services,IConfiguration configuration)
    {
                _services.AddDbContext<ProfileDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("ConnectionString not found in configuration."), npgsql =>
                npgsql.MigrationsHistoryTable("__ef_migrations_history_profile"))
                .UseSnakeCaseNamingConvention();
        });
        _services.AddScoped<IUnitOfWork, UnitOfWork>();
        _services.AddScoped<IProfileReadRepository, ProfileReadRepository>();
        _services.AddScoped<IProfileWriteRepository, ProfileWriteRepository>();
        _services.AddScoped<IProfileSettingsProvider, ProfileSettingsProvider>();
        _services.AddScoped<IEventHandler<UserRegisteredEvent>, UserRegisteredEventHandler>();
        return _services;
        
    }

}
