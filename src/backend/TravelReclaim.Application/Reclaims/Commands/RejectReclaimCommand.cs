using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Reclaims.Commands;

public record RejectReclaimCommand(Guid ReclaimId, string Reason, string ProcessedBy = "admin") : ICommand<ReclaimResponse>;