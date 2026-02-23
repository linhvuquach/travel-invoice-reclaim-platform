using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Application.DTOs;

public record ProcessReclaimResponse(
    ReclaimResponse? Reclaim,
    bool IsValid,
    IReadOnlyList<ValidationRuleResultDto> ValidationResults
);

public record ReclaimResponse(
    Guid Id,
    Guid InvoiceId,
    string InvoiceNumber,
    string HotelName,
    decimal EligibleAmount,
    string Status,
    DateTime CreatedDate,
    DateTime? ProcessedDate,
    string? RejectionReason,
    string? ProcessedBy
);

public record ValidationRuleResultDto(string RuleName, bool Passed, string? FailureReason);

public static class ReclaimMapper
{
    public static ReclaimResponse ToResponse(this Reclaim reclaim)
    {
        return new ReclaimResponse(
                Id: reclaim.Id,
                InvoiceId: reclaim.InvoiceId,
                InvoiceNumber: reclaim.Invoice?.InvoiceNumber ?? string.Empty,
                HotelName: reclaim.Invoice?.HotelName ?? string.Empty,
                EligibleAmount: reclaim.EligibleAmount,
                Status: reclaim.Status.ToString(),
                CreatedDate: reclaim.CreatedDate,
                ProcessedDate: reclaim.ProcessedDate,
                RejectionReason: reclaim.RejectionReason,
                ProcessedBy: reclaim.ProcessedBy
            );
    }
}