using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TravelReclaim.Domain;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Infrastructure.Persistence.SqlServer.Repositories;

public class InvoiceRepository : RepositoryBase<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Invoice?> GetByIdWithPaymentAsync(Guid id, CancellationToken ct = default)
    {
        return await Dbset
            .Include(i => i.Payment)
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Invoice> items, int TotalCount)> GetPagedAsync(InvoiceFilter filter, int page, int pageSize, CancellationToken ct = default)
    {
        var query = Dbset.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter.HotelName))
            query = query.Where(i => i.HotelName.Contains(filter.HotelName));

        if (filter.Status.HasValue)
            query = query.Where(i => i.Status == filter.Status.Value);

        if (filter.FromDate.HasValue)
            query = query.Where(i => i.SubmissionDate >= filter.FromDate.Value);

        if (filter.ToDate.HasValue)
            query = query.Where(i => i.SubmissionDate <= filter.ToDate.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(i => i.SubmissionDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(i => i.Payment)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}