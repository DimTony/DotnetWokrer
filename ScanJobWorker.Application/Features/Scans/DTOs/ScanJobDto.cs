namespace ScanJobWorker.Application.Features.Scans.DTOs;

public record ScanJobDto(
    Guid ScanId,
    Guid DomainId,
    string DomainName,
    string ScanType,
    Guid RequestedBy,
    DateTime EnqueuedAt
);