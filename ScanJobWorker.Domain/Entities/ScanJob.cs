namespace ScanJobWorker.Domain.Entities;

public sealed class ScanJob
{
    
    public Guid Id { get; private set; }
    public Guid DomainId { get; private set; }
    public string DomainName { get; private set; } = default!;
    public Guid ScanId { get; private set; }
    public string ScanType { get; private set; } = default!;
    public Guid RequestedBy { get; private set; }
    public DateTime EnqueuedAt { get; private set; }

    private ScanJob() { } 

    public static ScanJob Create(Guid domainId, string domainName, Guid scanId, string scanType, Guid requestedBy, DateTime enqueuedAt)
    {
        return new ScanJob
        {
            Id        = Guid.NewGuid(),
            DomainId    = domainId,
            DomainName = domainName,
            ScanId = scanId,
            ScanType = scanType,
            RequestedBy = requestedBy,
            EnqueuedAt = enqueuedAt
        };
    }
}