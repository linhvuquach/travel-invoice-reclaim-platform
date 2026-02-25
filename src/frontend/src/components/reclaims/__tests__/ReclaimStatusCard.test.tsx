import { render, screen } from "@testing-library/react";
import ReclaimStatusCard from "@/components/reclaims/ReclaimStatusCard";
import type { Reclaim } from "@/types/reclaim";

const baseReclaim: Reclaim = {
  id: "r1",
  invoiceId: "inv1",
  invoiceNumber: "INV-001",
  hotelName: "Grand Hotel",
  eligibleAmount: 2000,
  status: "Pending",
  createdDate: "2025-03-01T00:00:00Z",
};

describe("ReclaimStatusCard", () => {
  it("renders reclaim details", () => {
    render(<ReclaimStatusCard reclaim={baseReclaim} />);
    expect(screen.getByText("#INV-001")).toBeDefined();
    expect(screen.getByText("Grand Hotel")).toBeDefined();
    expect(screen.getByText("Pending")).toBeDefined();
  });

  it("shows rejection reason when present", () => {
    const reclaim: Reclaim = {
      ...baseReclaim,
      status: "Rejected",
      rejectionReason: "Invalid documentation",
    };
    render(<ReclaimStatusCard reclaim={reclaim} />);
    expect(screen.getByText("Invalid documentation")).toBeDefined();
  });

  it("shows processed date when present", () => {
    const reclaim: Reclaim = {
      ...baseReclaim,
      processedDate: "2025-03-05T00:00:00Z",
      processedBy: "admin",
    };
    render(<ReclaimStatusCard reclaim={reclaim} />);
    expect(screen.getByText("05 Mar 2025")).toBeDefined();
    expect(screen.getByText("admin")).toBeDefined();
  });
});
