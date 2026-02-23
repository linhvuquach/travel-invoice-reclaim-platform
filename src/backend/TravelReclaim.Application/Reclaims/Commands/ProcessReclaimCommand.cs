using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;

namespace TravelReclaim.Application.Reclaims.Commands;

public record ProcessReclaimCommand(Guid InvoiceId) : ICommand<ProcessReclaimResponse>;