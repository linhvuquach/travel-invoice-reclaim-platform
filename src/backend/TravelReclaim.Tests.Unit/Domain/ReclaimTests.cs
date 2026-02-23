using TravelReclaim.Domain.Entities;
using TravelReclaim.Domain.Enums;

namespace TravelReclaim.Tests.Unit.Domain;

public class ReclaimTests
{
    [Fact]
    public void Create_ValidParameters_ReturnsReclaimWithPendingStatus()
    {
        var invoiceId = Guid.NewGuid();
        var reclaim = Reclaim.Create(invoiceId, 190m);

        Assert.Equal(ReclaimStatus.Pending, reclaim.Status);
        Assert.Equal(invoiceId, reclaim.InvoiceId);
        Assert.Equal(190m, reclaim.EligibleAmount);
        Assert.NotEqual(Guid.Empty, reclaim.Id);
    }

    [Fact]
    public void Approve_FromPending_Succeeds()
    {
        var invoiceId = Guid.NewGuid();
        var reclaim = Reclaim.Create(invoiceId, 190m);

        reclaim.Approve("admin");

        Assert.Equal(ReclaimStatus.Approved, reclaim.Status);
        Assert.Equal("admin", reclaim.ProcessedBy);
        Assert.NotNull(reclaim.ProcessedDate);
    }

    [Fact]
    public void Reject_FromPending_Succeeds()
    {
        var invoiceId = Guid.NewGuid();
        var reclaim = Reclaim.Create(invoiceId, 190m);

        reclaim.Reject("Invalid doc", "admin");

        Assert.Equal(ReclaimStatus.Rejected, reclaim.Status);
        Assert.Equal("Invalid doc", reclaim.RejectionReason);
        Assert.Equal("admin", reclaim.ProcessedBy);
    }

    [Fact]
    public void Approve_FromRejected_ThrowsInvalidOperations()
    {
        var invoiceId = Guid.NewGuid();
        var reclaim = Reclaim.Create(invoiceId, 190m);
        reclaim.Reject("Invalid doc", "admin");

        Assert.Throws<InvalidOperationException>(() => reclaim.Approve("admin"));
    }

    [Fact]
    public void Reject_FromApproved_ThrowsInvalidOperation()
    {
        var invoiceId = Guid.NewGuid();
        var reclaim = Reclaim.Create(invoiceId, 190m);
        reclaim.Approve("admin");

        Assert.Throws<InvalidOperationException>(() => reclaim.Reject("Invalid doc", "admin"));
    }
}