namespace TravelReclaim.Domain.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<(IReadOnlyList<Invoice> items, int TotalCount)> GetPagedAsync(
        InvoiceFilter filter, int page, int pageSize, CancellationToken ct = default
    );

    Task<Invoice?> GetByIdWithPaymentAsync(Guid id, CancellationToken ct = default);
}

public record InvoiceFilter(
    string? HotelName = null,
    InvoiceStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
);