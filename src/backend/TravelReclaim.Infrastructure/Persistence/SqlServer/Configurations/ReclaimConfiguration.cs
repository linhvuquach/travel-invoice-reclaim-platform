using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelReclaim.Domain.Entities;

namespace TravelReclaim.Infrastructure.Persistence.SqlServer.Configurations;

public class ReclaimConfiguration : IEntityTypeConfiguration<Reclaim>
{
    public void Configure(EntityTypeBuilder<Reclaim> builder)
    {
        builder.ToTable("Reclaims");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(r => r.EligibleAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CreatedDate).IsRequired();
        builder.Property(r => r.RejectionReason).HasMaxLength(1000);
        builder.Property(r => r.ProcessedBy).HasMaxLength(200);

        // FK to Invoice - RESTRICT (reclaims have legal audit significance)
        builder.HasOne(r => r.Invoice)
            .WithMany()
            .HasForeignKey(r => r.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique index on InvoiceId (one reclaim per invoice)
        builder.HasIndex(r => r.InvoiceId).IsUnique();

        // Composite index for listing queries
        builder.HasIndex(r => new { r.Status, r.CreatedDate })
            .IsDescending(false, true);
    }
}