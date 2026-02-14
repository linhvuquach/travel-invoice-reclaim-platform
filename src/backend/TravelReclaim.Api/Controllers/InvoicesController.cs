using Microsoft.AspNetCore.Mvc;
using TravelReclaim.Application;
using TravelReclaim.Application.Common.CQRS;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Application.Invoices.Queries;
using TravelReclaim.Domain;

namespace TravelReclaim.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly ICommandHandler<CreateInvoiceCommand, InvoiceResponse> _createHandler;
    private readonly IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse> _getByIdHandler;
    private readonly IQueryHandler<GetInvoicesPagedQuery, PagedResponse<InvoiceResponse>> _getPagedHandler;

    public InvoicesController(
        ICommandHandler<CreateInvoiceCommand, InvoiceResponse> createHandler,
        IQueryHandler<GetInvoiceByIdQuery, InvoiceResponse> getbyIdHandler,
        IQueryHandler<GetInvoicesPagedQuery, PagedResponse<InvoiceResponse>> getPagedHanlder
    )
    {
        _createHandler = createHandler;
        _getByIdHandler = getbyIdHandler;
        _getPagedHandler = getPagedHanlder;
    }

    [HttpPost]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateInvoiceCommand command, CancellationToken ct
    )
    {
        var response = await _createHandler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var response = await _getByIdHandler.HandleAsync(new GetInvoiceByIdQuery(id), ct);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<InvoiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? hotelName = null,
        [FromQuery] InvoiceStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken ct = default)
    {
        var query = new GetInvoicesPagedQuery(page, pageSize, hotelName, status, fromDate, toDate);
        var response = await _getPagedHandler.HandleAsync(query, ct);
        return Ok(response);
    }
}