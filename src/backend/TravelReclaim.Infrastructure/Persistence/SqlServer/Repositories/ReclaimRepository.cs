using Microsoft.EntityFrameworkCore;
using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Enums;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Infrastructure.Persistence.SqlServer.Repositories;

public class ReclaimRepository(AppDbContext context) : RepositoryBase<Reclaim>(context), IReclaimRepository
{
    public async Task<Reclaim?> GetByIdWithInvoiceAsync(Guid id, CancellationToken ct = default)
    {
        return await Dbset
            .Include(r => r.Invoice)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<Reclaim?> GetByInvoiceIdAsync(Guid invoiceId, CancellationToken ct = default)
    {
        return await Dbset
            .Include(r => r.Invoice)
            .FirstOrDefaultAsync(r => r.InvoiceId == invoiceId, ct);
    }

    public async Task<(IReadOnlyList<Reclaim> Items, int TotalCount)> GetPagedAsync(ReclaimStatus? status, int page, int pageSize, CancellationToken ct = default)
    {
        var query = Dbset.AsNoTracking().Include(r => r.Invoice).AsQueryable();

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(r => r.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}