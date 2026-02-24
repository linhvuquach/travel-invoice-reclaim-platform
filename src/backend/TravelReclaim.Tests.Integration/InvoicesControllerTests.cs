using System.Net;
using System.Net.Http.Json;
using TravelReclaim.Application.DTOs;
using TravelReclaim.Application.Invoices.Commands;
using TravelReclaim.Tests.Integration.Helpers;

namespace TravelReclaim.Tests.Integration;

public class InvoicesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public InvoicesControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateInvoice_ValidRequest_Returns201()
    {
        // Given
        var command = new CreateInvoiceCommand(
            "Integration Hotel", InvoiceHelper.UniqueInvoiceNumber(), DateTime.UtcNow.AddDays(-5),
            1500m, 285m, "EUR", "Integration test invoice"
        );

        // When
        var response = await _client.PostAsJsonAsync("/api/v1/invoices", command);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
        Assert.NotNull(invoice);
        Assert.Equal("Integration Hotel", invoice.HotelName);
        Assert.Equal("Submitted", invoice.Status);
        Assert.NotNull(response.Headers.Location);
    }

    [Fact]
    public async Task GetInvoiceById_ExistingInvoice_Returns200()
    {
        // Given
        var command = new CreateInvoiceCommand(
            "Hotel A", InvoiceHelper.UniqueInvoiceNumber(), DateTime.UtcNow, 1000m, 190m, "EUR", null);
        var createResponse = await _client.PostAsJsonAsync("/api/v1/invoices", command);
        var created = await createResponse.Content.ReadFromJsonAsync<InvoiceResponse>();

        // When
        var response = await _client.GetAsync($"/api/v1/invoices/{created.Id}");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var invoice = await response.Content.ReadFromJsonAsync<InvoiceResponse>();
        Assert.Equal(created.Id, invoice.Id);
    }

    [Fact]
    public async Task GetInvoiceById_NonExistent_Returns404()
    {
        var response = await _client.GetAsync($"/api/v1/invoices/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetInvoicesPaged_ReturnsPagedResponse()
    {
        // Given
        for (int i = 0; i < 3; i++)
        {
            var cmd = new CreateInvoiceCommand(
                $"Paged Hotel {i}", InvoiceHelper.UniqueInvoiceNumber(), DateTime.UtcNow, 100m * (i + 1), 19m * (i + 1), "EUR", null);
            await _client.PostAsJsonAsync("/api/v1/invoices", cmd);
        }

        // When
        var response = await _client.GetAsync("/api/v1/invoices?page=1&pageSize=10");

        // Then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var paged = await response.Content.ReadFromJsonAsync<PagedResponse<InvoiceResponse>>();
        Assert.NotNull(paged);
        Assert.True(paged.Items.Count >= 3);
        Assert.Equal(1, paged.Page);
    }
}
