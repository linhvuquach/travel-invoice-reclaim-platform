namespace TravelReclaim.Tests.Integration.Helpers;

public static class InvoiceHelper
{
    public static string UniqueInvoiceNumber() => $"INV-{Guid.NewGuid():N}"[..20];
}