import { render, screen } from "@testing-library/react";
import ValidationResults from "@/components/reclaims/ValidationResults";
import type { ValidationRuleResult } from "@/types/reclaim";

describe("ValidationResults", () => {
  it("renders nothing when results are empty", () => {
    const { container } = render(<ValidationResults results={[]} />);
    expect(container.innerHTML).toBe("");
  });

  it("renders passed rules with check mark", () => {
    const results: ValidationRuleResult[] = [
      { ruleName: "AmountThreshold", passed: true },
    ];
    render(<ValidationResults results={results} />);
    expect(screen.getByText("\u2713")).toBeDefined();
    expect(screen.getByText("Amount Threshold")).toBeDefined();
  });

  it("renders failed rules with cross mark and reason", () => {
    const results: ValidationRuleResult[] = [
      {
        ruleName: "VatRatio",
        passed: false,
        failureReason: "VAT exceeds 25%",
      },
    ];
    render(<ValidationResults results={results} />);
    expect(screen.getByText("\u2717")).toBeDefined();
    expect(screen.getByText("Vat Ratio")).toBeDefined();
    expect(screen.getByText("VAT exceeds 25%")).toBeDefined();
  });

  it("renders mixed results correctly", () => {
    const results: ValidationRuleResult[] = [
      { ruleName: "AmountThreshold", passed: true },
      {
        ruleName: "DuplicateInvoice",
        passed: false,
        failureReason: "Duplicate found",
      },
    ];
    render(<ValidationResults results={results} />);
    expect(screen.getByText("Amount Threshold")).toBeDefined();
    expect(screen.getByText("Duplicate Invoice")).toBeDefined();
    expect(screen.getByText("Duplicate found")).toBeDefined();
  });
});
