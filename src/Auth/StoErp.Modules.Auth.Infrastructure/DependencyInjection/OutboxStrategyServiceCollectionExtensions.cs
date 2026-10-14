using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using StoErp.Modules.Auth.Infrastructure.Persistence.Outbox.Writer;
using StoErp.Shared.Application.Events;

namespace StoErp.Modules.Auth.Infrastructure.DependencyInjection.Configurations;

public static class OutboxStrategyServiceCollectionExtensions
{
    public static IServiceCollection AddOutboxWriterStrategies(this IServiceCollection services, Assembly assembly)
    {
        var strategyTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IOutboxEventWriterStrategy).IsAssignableFrom(t));
        foreach (var strategyType in strategyTypes)
        {
            services.AddScoped(typeof(IOutboxEventWriterStrategy), strategyType);
        }
        return services;
    }
    public static IServiceCollection AddOutboxReaderStrategies(this IServiceCollection services, Assembly assembly)
    {
        var strategyTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IOutboxEventReaderStrategy).IsAssignableFrom(t));
        foreach (var strategyType in strategyTypes)
        {
            services.AddScoped(typeof(IOutboxEventReaderStrategy), strategyType);
        }
        return services;
    }
}
