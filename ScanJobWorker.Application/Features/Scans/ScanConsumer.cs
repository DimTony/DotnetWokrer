using Microsoft.Extensions.Logging;
using ScanJobWorker.Application.Features.Scans.DTOs;
using ScanJobWorker.Domain.Entities;

namespace ScanJobWorker.Application.Features.Scans;
public sealed class ScanConsumer
{
    private readonly ILogger<ScanConsumer> _logger;

    public ScanConsumer(ILogger<ScanConsumer> logger)
        => _logger = logger;

    public Task ExecuteAsync(ScanJobDto dto, CancellationToken ct)
    {
        var job = ScanJob.Create(
            dto.DomainId,
            dto.DomainName,
            dto.ScanId,
            dto.ScanType,
            dto.RequestedBy,
            dto.EnqueuedAt
        );

        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["JobId"]  = job.Id,
            ["Target"] = job.DomainName
        });

        _logger.LogInformation(
            "Processing scan job {JobId} → {Target} | Types: [{Types}]",
            job.Id,
            job.DomainName,
            string.Join(", ", job.ScanType)
        );

        // TODO: dispatch to scanner modules
        return Task.CompletedTask;
    }
}