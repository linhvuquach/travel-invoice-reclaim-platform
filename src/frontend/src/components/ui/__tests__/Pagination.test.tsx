import { fireEvent, render, screen } from "@testing-library/react";
import Pagination from "../Pagination";

describe("Pagination", () => {
  it("renders page into correctly", () => {
    render(<Pagination page={2} totalPages={5} onPageChange={() => {}} />);
    expect(screen.getByText("Page 2 of 5")).toBeDefined();
  });

  it("disables Previous button on first page", () => {
    render(<Pagination page={1} totalPages={5} onPageChange={() => {}} />);
    const prevButton = screen.getByText("Previous") as HTMLButtonElement;
    expect(prevButton.disabled).toBe(true);
  });

  it("disables Next button on last page", () => {
    render(<Pagination page={5} totalPages={5} onPageChange={() => {}} />);
    const nextButton = screen.getByText("Next") as HTMLButtonElement;
    expect(nextButton.disabled).toBe(true);
  });

  it("calls onPageChagne with correct pge on Next click", () => {
    const onPageChagne = jest.fn();
    render(<Pagination page={1} totalPages={5} onPageChange={onPageChagne} />);
    fireEvent.click(screen.getByText("Next"));
    expect(onPageChagne).toHaveBeenCalledWith(2);
  });

  it("calls onPageChange with correct page on Previous click", () => {
    const onPageChange = jest.fn();
    render(<Pagination page={3} totalPages={5} onPageChange={onPageChange} />);
    fireEvent.click(screen.getByText("Previous"));
    expect(onPageChange).toHaveBeenCalledWith(2);
  });
});
