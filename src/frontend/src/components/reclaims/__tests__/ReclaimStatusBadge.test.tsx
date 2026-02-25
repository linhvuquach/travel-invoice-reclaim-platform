import { render, screen } from "@testing-library/react";
import ReclaimStatusBadge from "../ReclaimStatusBadge";

describe("ReclaimStatusBadge", () => {
  it.each(["Pending", "Approved", "Rejected", "Paid"])(
    "render %s status text",
    (status) => {
      render(<ReclaimStatusBadge status={status} />);
      expect(screen.getByText(status)).toBeDefined();
    },
  );

  it("renders unknown status with fallback styling", () => {
    render(<ReclaimStatusBadge status="Custom" />);
    expect(screen.getByText("Custom")).toBeDefined();
  });
});
