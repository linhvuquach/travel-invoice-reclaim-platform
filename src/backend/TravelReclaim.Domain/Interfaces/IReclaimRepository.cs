using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Enums;

namespace TravelReclaim.Domain.Interfaces;

public interface IReclaimRepository : IRepository<Reclaim>
{
    Task<Reclaim?> GetByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default);
    Task<Reclaim?> GetByIdWithInvoiceAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Reclaim> Items, int TotalCount)> GetPagedAsync(
        ReclaimStatus? status, int page, int pageSize, CancellationToken ct = default
    );
}