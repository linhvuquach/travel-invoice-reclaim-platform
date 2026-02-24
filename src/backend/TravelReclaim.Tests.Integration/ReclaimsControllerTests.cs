using System.Net;
using System.Net.Http.Json;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Tests.Integration.Helpers;

namespace TravelReclaim.Tests.Integration;

public class ReclaimsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReclaimsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ProcessReclaim_ValidSubmittedInvoice_Returns200()
    {
        // Given
        var command = new CreateInvoiceCommand(
            "Reclaim Hotel", InvoiceHelper.UniqueInvoiceNumber(), DateTime.UtcNow.AddDays(-5),
            2000m, 380m, "EUR", "Test reclaim");
        var createResponse = await _client.PostAsJsonAsync("/api/v1/invoices", command);
        var invoice = await createResponse.Content.ReadFromJsonAsync<InvoiceResponse>();

        // When
        var response = await _client.PostAsync($"/api/v1/reclaims/{invoice.Id}/process", null);

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<ProcessReclaimResponse>();
        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.NotNull(result.Reclaim);
        Assert.Equal("Pending", result.Reclaim.Status);
    }

    [Fact]
    public async Task ProcessReclaim_NonExistentInvoice_Returns404()
    {
        // When
        var response = await _client.PostAsync($"/api/v1/reclaims/{Guid.NewGuid()}/process", null);

        // Then
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetReclaimsPaged_ReturnsPagedResponse()
    {
        // When
        var response = await _client.GetAsync("/api/v1/reclaims?page=1&pageSize=10");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<ProcessReclaimResponse>>();
        Assert.NotNull(paged);
        Assert.Equal(1, paged.Page);
    }
}
