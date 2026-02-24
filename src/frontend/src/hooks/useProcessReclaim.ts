import { processReclaim } from "@/lib/api/reclaims";
import { ProcessReclaimResponse } from "@/types/reclaim";
import { useCallback, useState } from "react";

export function useProcessReclaim() {
  const [result, setResult] = useState<ProcessReclaimResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const process = useCallback(async (invoiceId: string) => {
    setLoading(true);
    setError(null);
    setResult(null);

    try {
      const data = await processReclaim(invoiceId);
      setResult(data);
      return data;
    } catch (err) {
      const message =
        err instanceof Error ? err.message : "Failed to process reclaim";
      setError(message);
      return null;
    } finally {
      setLoading(false);
    }
  }, []);

  const reset = useCallback(() => {
    setResult(null);
    setError(null);
  }, []);

  return { result, loading, error, process, reset };
}
