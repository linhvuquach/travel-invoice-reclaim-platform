using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Invoices.Queries;

public class GetInvoiceByIdHandler(IInvoiceRepository invoiceRepository) : IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse>
{
    public async Task<InvoiceResponse> HandleAsync(GetInvoiceByIdQuery query, CancellationToken ct = default)
    {
        var invoice = await invoiceRepository.GetByIdWithPaymentAsync(query.Id, ct)
            ?? throw new EntityNotFoundException(nameof(Domain.Invoice), query.Id);

        return invoice.ToResponse();
    }
}