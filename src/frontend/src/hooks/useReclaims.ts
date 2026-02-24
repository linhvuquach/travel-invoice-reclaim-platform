import { getReclaims } from "@/lib/api/reclaims";
import { PagedReclaimResponse } from "@/types/reclaim";
import { useCallback, useEffect, useState } from "react";

export function useReclaims(initialPage = 1, pageSize = 10) {
  const [data, setData] = useState<PagedReclaimResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(initialPage);
  const [statusFilter, setStatusFilter] = useState<string | undefined>();
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
        const result = await getReclaims({
          page,
          pageSize,
          status: statusFilter,
        });
        if (!cancelled) {
          setData(result);
        }
      } catch (err) {
        if (!cancelled) {
          setError(
            err instanceof Error ? err.message : "Failed to load reclaims",
          );
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    fetchData();

    return () => {
      cancelled = true;
    };
  }, [page, pageSize, statusFilter, refreshKey]);

  return {
    data,
    loading,
    error,
    page,
    setPage,
    statusFilter,
    setStatusFilter,
    refetch,
  };
}
