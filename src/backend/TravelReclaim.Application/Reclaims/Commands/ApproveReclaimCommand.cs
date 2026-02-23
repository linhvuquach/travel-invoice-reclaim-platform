using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Reclaims.Commands;

public record ApproveReclaimCommand(Guid ReclaimId, string ProcessedBy = "admin") : ICommand<ReclaimResponse>;