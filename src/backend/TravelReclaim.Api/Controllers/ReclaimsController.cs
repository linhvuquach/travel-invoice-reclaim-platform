using Microsoft.AspNetCore.Mvc;
using TravelReclaim.Application;
using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Reclaims.Commands;
using TravelReclaim.Application.Reclaims.Queries;
using TravelReclaim.Domain.Enums;

namespace TravelReclaim.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ReclaimsController(
    ICommandHandler<ProcessReclaimCommand, ProcessReclaimResponse> processHandler,
    ICommandHandler<ApproveReclaimCommand, ReclaimResponse> approveHandler,
    ICommandHandler<RejectReclaimCommand, ReclaimResponse> rejectHandler,
    IQueryHandler<GetReclaimByIdQuery, ReclaimResponse> getByIdHandler,
    IQueryHandler<GetReclaimsPagedQuery, PagedResponse<ReclaimResponse>> getPagedHandler
) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReclaimResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var response = await getByIdHandler.HandleAsync(new GetReclaimByIdQuery(id), ct);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ReclaimResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] ReclaimStatus? status = null,
        CancellationToken ct = default)
    {
        var query = new GetReclaimsPagedQuery(page, pageSize, status);
        var response = await getPagedHandler.HandleAsync(query, ct);
        return Ok(response);
    }

    [HttpPost("{invoiceId:guid}/process")]
    [ProducesResponseType(typeof(ProcessReclaimResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Process(Guid invoiceId, CancellationToken ct)
    {
        var response = await processHandler.HandleAsync(new ProcessReclaimCommand(invoiceId), ct);
        return Ok(response);
    }

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(ReclaimResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
    {
        var response = await approveHandler.HandleAsync(new ApproveReclaimCommand(id), ct);
        return Ok(response);
    }

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(ReclaimResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectReclaimRequest request, CancellationToken ct)
    {
        var response = await rejectHandler.HandleAsync(new RejectReclaimCommand(id, request.Reason), ct);
        return Ok(response);
    }
}

public record RejectReclaimRequest(string Reason);