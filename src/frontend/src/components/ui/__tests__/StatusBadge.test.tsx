import { render, screen } from "@testing-library/react";
import StatusBadge from "@/components/ui/StatusBadge";

describe("StatusBadge", () => {
  it("renders the status text", () => {
    render(<StatusBadge status="Submitted" />);
    expect(screen.getByText("Submitted")).toBeDefined();
  });

  it("renders unknown status with default styling", () => {
    render(<StatusBadge status="Unknown" />);
    expect(screen.getByText("Unknown")).toBeDefined();
  });
});
