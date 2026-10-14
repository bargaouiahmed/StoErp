using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StoErp.Shared.Infrastructure.Messaging.Outbox;
namespace StoErp.Shared.Infrastructure.Messaging;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private static readonly TimeSpan _defaultDelay = TimeSpan.FromSeconds(5);
    private const int BatchSize = 50;
    public OutboxProcessor(IServiceScopeFactory serviceScopeFactory, ILogger<OutboxProcessor> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_defaultDelay);
        await ProcessBatchAsync(stoppingToken);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ProcessBatchAsync(stoppingToken);
        }

    }


    private async Task ProcessBatchAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var outboxDispatcher = scope.ServiceProvider.GetRequiredService<OutboxDispatcher>();
        try
        {
            await outboxDispatcher.DispatchBatchAsync(BatchSize, stoppingToken);
        }
        catch (OperationCanceledException)
        when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Outbox processing was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the outbox messages.");
        }
    }


}
