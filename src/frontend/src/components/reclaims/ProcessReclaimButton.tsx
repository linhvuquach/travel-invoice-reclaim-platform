import { useProcessReclaim } from "@/hooks/useProcessReclaim";
import styles from "@/styles/reclaims.module.css";
import ValidationResults from "./ValidationResults";
import { useRouter } from "next/navigation";

interface ProcessReclaimButtonProps {
  invoiceId: string;
  invoiceStatus: string;
}

export default function ProcessReclaimButton({
  invoiceId,
  invoiceStatus,
}: ProcessReclaimButtonProps) {
  const router = useRouter();
  const { result, loading, error, process } = useProcessReclaim();

  const canProcess = invoiceStatus === "Submitted";
  const handleProcess = async () => {
    const data = await process(invoiceId);
    if (data?.reclaim) router.push(`/reclaims/${data.reclaim.id}`);
  };

  return (
    <div style={{ marginTop: "1.5rem" }}>
      <button
        className={styles.processButton}
        onClick={handleProcess}
        disabled={loading || !canProcess}
      >
        {loading ? "Processing..." : "Process Reclaim"}
      </button>

      {!canProcess &&
        invoiceStatus !== "ReclaimPending" &&
        invoiceStatus !== "Reclaimed" && (
          <p
            style={{
              color: "#6b7280",
              fontSize: "0.8rem",
              marginTop: "0.25rem",
            }}
          >
            Invoice must be in &quot;Submitted&quot; status to process a reclaim
          </p>
        )}

      {error && (
        <div className={`${styles.resultBanner} ${styles.resultFailure}`}>
          <p className={styles.resultTitle}>Processing Failed</p>
          <p style={{ margin: 0, fontSize: "0.875rem" }}>{error}</p>
        </div>
      )}

      {result && !result.isValid && (
        <div>
          <div className={`${styles.resultBanner} ${styles.resultFailure}`}>
            <p className={styles.resultTitle}>Validation Failed</p>
            <p style={{ margin: 0, fontSize: "0.875rem" }}>
              The invoice did not pass all validation rules.
            </p>
          </div>
          <ValidationResults results={result.validationResults} />
        </div>
      )}

      {result && result.isValid && result.reclaim && (
        <div className={`${styles.resultBanner} ${styles.resultSuccess}`}>
          <p className={styles.resultTitle}>Reclaim Created Successfully</p>
          <p style={{ margin: 0, fontSize: "0.875rem" }}>
            Redirecting to reclaim details...
          </p>
        </div>
      )}
    </div>
  );
}
