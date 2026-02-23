using TravelReclaim.Application.DTOs;
using TravelReclaim.Domain.Enums;

namespace TravelReclaim.Application.Reclaims.Queries;

public record GetReclaimsPagedQuery(
    int Page = 1,
    int PageSize = 20,
    ReclaimStatus? Status = null
) : IQuery<PagedResponse<ReclaimResponse>>;