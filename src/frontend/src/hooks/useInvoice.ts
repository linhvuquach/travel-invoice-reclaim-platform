"use-client";

import { getInvoiceByid } from "@/lib/api/invoices";
import { Invoice } from "@/types/invoice";
import { useEffect, useState } from "react";

export function useInvoice(id: string) {
  const [invoice, setInvoice] = useState<Invoice | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function fetchInvoice() {
      setLoading(true);
      setError(null);

      try {
        const data = await getInvoiceByid(id);
        if (!cancelled) setInvoice(data);
      } catch (error) {
        if (!cancelled)
          setError(
            error instanceof Error ? error.message : "Failed to load invoice",
          );
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    fetchInvoice();

    return () => {
      cancelled = true;
    };
  }, [id]);

  return { invoice, loading, error };
}
