using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Invoices.Queries;

public class GetInvoicesPagedHandler(IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoicesPagedQuery, PagedResponse<InvoiceResponse>>
{
    public async Task<PagedResponse<InvoiceResponse>> HandleAsync(GetInvoicesPagedQuery query, CancellationToken ct = default)
    {
        var filter = new InvoiceFilter(
            HotelName: query.HotelName,
            Status: query.Status,
            FromDate: query.FromDate,
            ToDate: query.ToDate);

        var (items, totalCount) = await invoiceRepository.GetPagedAsync(
            filter, query.Page, query.PageSize, ct);

        var responses = items.Select(InvoiceMapper.ToResponse).ToList();
        var totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new PagedResponse<InvoiceResponse>(
            Items: responses,
            Page: query.Page,
            PageSize: query.PageSize,
            TotalCount: totalCount,
            TotalPages: totalPages
        );
    }
}