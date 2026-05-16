using StackExchange.Redis;
using System.Text.Json;
using ScanJobWorker.Application.Interfaces;
using ScanJobWorker.Application.Features.Scans.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ScanJobWorker.Infrastructure.Services;


public sealed class RedisService : IRedisService
{
    private readonly IDatabase _db;
    private readonly string _queueKey;
    private readonly ILogger<RedisService> _logger;

    public RedisService(IConnectionMultiplexer redis, IConfiguration config, ILogger<RedisService> logger)
    {
        _db       = redis.GetDatabase();
        _queueKey = config["Redis:QueueKey"] ?? "scan:jobs";
        _logger   = logger;

        _logger.LogInformation("RedisService listening on queue: {QueueKey}", _queueKey); // ← add this
    }

    public async Task<ScanJobDto?> DequeueAsync(CancellationToken ct)
    {
        var value = await _db.ListLeftPopAsync(_queueKey);

        if (value.IsNullOrEmpty)
        {
            _logger.LogDebug("Queue '{QueueKey}' is empty, no job found", _queueKey); // ← add this
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<ScanJobDto>(value!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Bad job payload discarded: {Raw}", (string)value!);
            return null;
        }
    }
}