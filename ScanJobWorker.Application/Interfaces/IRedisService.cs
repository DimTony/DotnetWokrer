using ScanJobWorker.Application.Features.Scans.DTOs;

namespace ScanJobWorker.Application.Interfaces;

public interface IRedisService
{
    Task<ScanJobDto?> DequeueAsync(CancellationToken ct);
}