export function formatCurrency(amout: number, currency: string): string {
  return new Intl.NumberFormat("en-UI", {
    style: "currency",
    currency,
  }).format(amout);
}

export function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleDateString("en-GB", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
}
