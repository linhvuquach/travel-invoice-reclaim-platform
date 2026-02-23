using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain.Interfaces;

namespace TravelReclaim.Application.Reclaims.Queries;

public class GetReclaimsPagedHandler(IReclaimRepository reclaimRepository) : IQueryHandler<GetReclaimsPagedQuery, PagedResponse<ReclaimResponse>>
{
    public async Task<PagedResponse<ReclaimResponse>> HandleAsync(GetReclaimsPagedQuery query, CancellationToken ct = default)
    {
        var (items, totalCount) = await reclaimRepository.GetPagedAsync(query.Status, query.Page, query.PageSize, ct);

        var responses = items.Select(ReclaimMapper.ToResponse).ToList();

        return new PagedResponse<ReclaimResponse>(
            Items: responses,
            Page: query.Page,
            PageSize: query.PageSize,
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling((double)totalCount / query.PageSize)
        );
    }
}