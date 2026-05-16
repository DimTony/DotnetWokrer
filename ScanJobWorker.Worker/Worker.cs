using ScanJobWorker.Application.Features.Scans;
using ScanJobWorker.Application.Interfaces;

namespace ScanJobWorker.Worker;

public sealed class Worker : BackgroundService
{
    private readonly IRedisService _redis;
    private readonly ScanConsumer _consumer;
    private readonly ILogger<Worker> _logger;

    public Worker(IRedisService redis, ScanConsumer consumer, ILogger<Worker> logger)
    {
        _redis   = redis;
        _consumer = consumer;
        _logger  = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var job = await _redis.DequeueAsync(stoppingToken);

                if (job is null) { await Task.Delay(500, stoppingToken); continue; }

                await _consumer.ExecuteAsync(job, stoppingToken);
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Worker loop error");
                await Task.Delay(2000, stoppingToken);
            }
        }

        _logger.LogInformation("Worker stopped");
    }
}
