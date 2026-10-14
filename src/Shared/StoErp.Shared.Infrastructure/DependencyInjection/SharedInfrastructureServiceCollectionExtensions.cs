
using Microsoft.Extensions.DependencyInjection;
using StoErp.Shared.Infrastructure.Messaging;
using StoErp.Shared.Infrastructure.Messaging.Outbox;

namespace StoErp.Shared.Infrastructure.DependencyInjection;

public static class SharedInfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<OutboxDispatcher>();
        services.AddHostedService<OutboxProcessor>();
        return services;
    }

}
