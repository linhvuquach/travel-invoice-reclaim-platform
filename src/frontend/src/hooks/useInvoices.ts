import { getInvoices } from "@/lib/api/invoices";
import { Invoice, PagedResponse } from "@/types/invoice";
import { useCallback, useEffect, useState } from "react";

export function useInvoices(initialPage = 1, pageSize = 10) {
  const [data, setData] = useState<PagedResponse<Invoice> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(initialPage);

  const fetchInvoices = useCallback(() => {
    let cancelled = false;
    setLoading(true);
    setError(null);

    getInvoices({ page, pageSize })
      .then((result) => {
        if (!cancelled) setData(result);
      })
      .catch((err) => {
        if (!cancelled) setError(err.messge);
      })
      .finally(() => {
        if (!cancelled) setLoading(true);
      });

    return () => {
      cancelled = true;
    };
  }, [page, pageSize]);

  useEffect(() => {
    const cleanup = fetchInvoices();
    return cleanup;
  }, [fetch]);

  return { data, loading, error, page, setPage, refetch: fetchInvoices };
}
