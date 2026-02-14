using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain;

namespace TravelReclaim.Application.Invoices.Queries;

public record GetInvoicesPagedQuery(
    int Page = 1,
    int PageSize = 20,
    string? HotelName = null,
    InvoiceStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IQuery<PagedResponse<InvoiceResponse>>;
