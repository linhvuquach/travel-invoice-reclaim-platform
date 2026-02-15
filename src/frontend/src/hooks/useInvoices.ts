"use-client";

import { getInvoices } from "@/lib/api/invoices";
import { Invoice, PagedResponse } from "@/types/invoice";
import { useCallback, useEffect, useState } from "react";

export function useInvoices(initialPage = 1, pageSize = 10) {
  const [data, setData] = useState<PagedResponse<Invoice> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(initialPage);
  const [refreshKey, setRefreshKey] = useState(0);

  const refetch = useCallback(() => {
    setRefreshKey((k) => k + 1);
  }, []);

  useEffect(() => {
    let cancelled = false;

    async function fetchData() {
      setLoading(true);
      setError(null);

      try {
        const result = await getInvoices({ page, pageSize });
        if (!cancelled) setData(result);
      } catch (error) {
        if (!cancelled)
          setError(
            error instanceof Error ? error.message : "Failed to load invoices",
          );
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    fetchData();

    return () => {
      cancelled = true;
    };
  }, [page, pageSize, refreshKey]);

  return { data, loading, error, page, setPage, refetch };
}
