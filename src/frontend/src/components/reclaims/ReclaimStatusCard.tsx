import { Reclaim } from "@/types/reclaim";
import styles from "@/styles/detail.module.css";
import DetailField from "../ui/DetailField";
import { formatCurrency, formatDate } from "@/utils";
import ReclaimStatusBadge from "./ReclaimStatusBadge";

interface ReclaimStatusCardProps {
  reclaim: Reclaim;
}

export default function ReclaimStatusCard({ reclaim }: ReclaimStatusCardProps) {
  return (
    <div className={styles.detailCard}>
      <div className={styles.detailGrid}>
        <DetailField label="Invoice" value={`#${reclaim.invoiceNumber}`} />
        <DetailField label="Hotel" value={reclaim.hotelName} />
        <DetailField
          label="Eligible Amount"
          value={formatCurrency(reclaim.eligibleAmount, "EUR")}
          variant="amount"
        />
        <DetailField label="Created" value={formatDate(reclaim.createdDate)} />
        {reclaim.processedDate && (
          <DetailField
            label="Processed"
            value={formatDate(reclaim.processedDate)}
          />
        )}
        {reclaim.processedBy && (
          <DetailField label="Processed By" value={reclaim.processedBy} />
        )}
        {reclaim.rejectionReason && (
          <DetailField
            label="Rejection Reason"
            value={reclaim.rejectionReason}
            fullWidth
          />
        )}
      </div>
      <div style={{ marginTop: "1rem" }}>
        <ReclaimStatusBadge status={reclaim.status} />
      </div>
    </div>
  );
}
