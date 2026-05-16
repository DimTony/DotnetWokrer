using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScanJobWorker.Application.Interfaces;
using ScanJobWorker.Infrastructure.Services;
using StackExchange.Redis;

namespace ScanJobWorker.Infrastructure;


public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(
                config.GetConnectionString("Redis") ?? "localhost:6379"));

        services.AddSingleton<IRedisService, RedisService>();
        return services;
    }
}
