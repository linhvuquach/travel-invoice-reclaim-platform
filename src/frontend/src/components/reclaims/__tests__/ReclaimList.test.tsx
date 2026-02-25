import { render, screen } from "@testing-library/react";
import ReclaimList from "../ReclaimList";
import { Reclaim } from "@/types/reclaim";

// Mock
jest.mock("next/link", () => {
  return function MockLink({
    children,
    href,
  }: {
    children: React.ReactNode;
    href: string;
  }) {
    return <a href={href}>{children}</a>;
  };
});

const reclaims: Reclaim[] = [
  {
    id: "r1",
    invoiceId: "inv1",
    invoiceNumber: "INV-001",
    hotelName: "Hotel Alpha",
    eligibleAmount: 1500,
    status: "Pending",
    createdDate: "2025-03-01T00:00:00Z",
  },
  {
    id: "r2",
    invoiceId: "inv2",
    invoiceNumber: "INV-002",
    hotelName: "Hotel Beta",
    eligibleAmount: 3000,
    status: "Approved",
    createdDate: "2025-03-02T00:00:00Z",
  },
];

describe("ReclaimLlist", () => {
  it("renders all reclaim cards", () => {
    render(<ReclaimList reclaims={reclaims} />);
    expect(screen.getByText("Hotel Alpha")).toBeDefined();
    expect(screen.getByText("Hotel Beta")).toBeDefined();
  });

  it("renders correct links", () => {
    render(<ReclaimList reclaims={reclaims} />);
    const links = screen.getAllByRole("link");
    expect(links[0].getAttribute("href")).toBe("/reclaims/r1");
    expect(links[1].getAttribute("href")).toBe("/reclaims/r2");
  });

  it("renders status badges", () => {
    render(<ReclaimList reclaims={reclaims} />);
    expect(screen.getByText("Pending")).toBeDefined();
    expect(screen.getByText("Approved")).toBeDefined();
  });

  it("renders invoice numbers", () => {
    render(<ReclaimList reclaims={reclaims} />);
    expect(screen.getByText("#INV-001")).toBeDefined();
    expect(screen.getByText("#INV-002")).toBeDefined();
  });
});
