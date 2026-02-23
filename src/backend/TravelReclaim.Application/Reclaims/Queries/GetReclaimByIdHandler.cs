using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Exceptions;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Reclaims.Queries;

public class GetReclaimByIdHandler(IReclaimRepository reclaimRepository) : IQueryHandler<GetReclaimByIdQuery, ReclaimResponse>
{
    public async Task<ReclaimResponse> HandleAsync(GetReclaimByIdQuery query, CancellationToken ct = default)
    {
        var reclaim = await reclaimRepository.GetByIdWithInvoiceAsync(query.Id, ct)
            ?? throw new EntityNotFoundException("Reclaim", query.Id);

        return reclaim.ToResponse();
    }
}