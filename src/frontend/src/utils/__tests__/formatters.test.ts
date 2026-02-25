import { formatCurrency, formatDate } from "@/utils";

describe("formatCurrency", () => {
  it("formats EUR amount correctly", () => {
    const result = formatCurrency(1500, "EUR");
    expect(result).toContain("1,500");
  });

  it("formats zero amount", () => {
    const result = formatCurrency(0, "EUR");
    expect(result).toContain("0");
  });

  it("formats decimal amounts", () => {
    const result = formatCurrency(99.5, "EUR");
    expect(result).toContain("99.50");
  });
});

describe("formatDate", () => {
  it("formats ISO date string to en-GB format", () => {
    const result = formatDate("2025-03-15T00:00:00Z");
    expect(result).toBe("15 Mar 2025");
  });

  it("formats date-only string", () => {
    const result = formatDate("2025-01-01");
    expect(result).toContain("Jan");
    expect(result).toContain("2025");
  });
});
