"use client";

import { useState, useCallback } from "react";
import { useRouter } from "next/navigation";
import { approveReclaim, rejectReclaim } from "@/lib/api/reclaims";
import styles from "@/styles/reclaims.module.css";

interface ReclaimActionsProps {
  reclaimId: string;
  status: string;
}

export default function ReclaimActions({
  reclaimId,
  status,
}: ReclaimActionsProps) {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showRejectDialog, setShowRejectDialog] = useState(false);
  const [rejectReason, setRejectReason] = useState("");

  const handleApprove = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      await approveReclaim(reclaimId);
      router.refresh();
      window.location.reload();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to approve");
    } finally {
      setLoading(false);
    }
  }, [reclaimId, router]);

  const handleReject = useCallback(async () => {
    if (!rejectReason.trim()) return;

    setLoading(true);
    setError(null);
    try {
      await rejectReclaim(reclaimId, rejectReason);
      router.refresh();
      window.location.reload();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to reject");
    } finally {
      setLoading(false);
    }
  }, [reclaimId, rejectReason, router]);

  if (status !== "Pending") return null;

  return (
    <div>
      <div className={styles.actionGroup}>
        <button
          className={styles.approveButton}
          onClick={handleApprove}
          disabled={loading}
        >
          {loading ? "Processing..." : "Approve"}
        </button>
        <button
          className={styles.rejectButton}
          onClick={() => setShowRejectDialog(true)}
          disabled={loading}
        >
          Reject
        </button>
      </div>

      {showRejectDialog && (
        <div className={styles.rejectDialog}>
          <textarea
            className={styles.rejectTextarea}
            placeholder="Enter rejection reason..."
            value={rejectReason}
            onChange={(e) => setRejectReason(e.target.value)}
          />
          <div className={styles.rejectDialogActions}>
            <button
              className={styles.rejectButton}
              onClick={handleReject}
              disabled={loading || !rejectReason.trim()}
            >
              Confirm Rejection
            </button>
            <button
              className={styles.cancelButton}
              onClick={() => {
                setShowRejectDialog(false);
                setRejectReason("");
              }}
            >
              Cancel
            </button>
          </div>
        </div>
      )}

      {error && (
        <p
          style={{
            color: "#dc2626",
            fontSize: "0.875rem",
            marginTop: "0.5rem",
          }}
        >
          {error}
        </p>
      )}
    </div>
  );
}
