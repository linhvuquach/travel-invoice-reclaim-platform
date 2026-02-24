import { getReclaimById } from "@/lib/api/reclaims";
import { Reclaim } from "@/types/reclaim";
import { useEffect, useState } from "react";

export function useReclaim(id: string) {
  const [reclaim, setReclaim] = useState<Reclaim | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // This pattern is commonly referred to as the "cancellation flag" or "subscription guard" pattern.
  // Technical patterns/keywords: "async effect cleanup", "cancellation flag", "prevent state updates on unmounted"
  useEffect(() => {
    let cancelled = false;

    async function fetchReclaim() {
      setLoading(true);
      setError(null);

      try {
        const data = await getReclaimById(id);
        if (!cancelled) setReclaim(data);
      } catch (error) {
        if (!cancelled)
          setError(
            error instanceof Error ? error.message : "Failed to load reclaim",
          );
      } finally {
        if (!cancelled) setLoading(false);
      }
    }

    fetchReclaim();

    return () => {
      cancelled = true;
    };
  }, [id]);

  return { reclaim, loading, error };
}
