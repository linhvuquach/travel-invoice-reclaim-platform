using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Reclaims.Queries;

public record GetReclaimByIdQuery(Guid Id) : IQuery<ReclaimResponse>;