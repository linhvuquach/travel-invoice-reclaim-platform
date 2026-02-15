import styles from "@/styles/invoices.module.css";

const statusClassMap: Record<string, string> = {
  Draft: styles.badgeDraft,
  Submitted: styles.badgeSubmitted,
  Validated: styles.badgeValidated,
  Rejected: styles.badgeRejected,
  ReclaimPending: styles.badgeReclaimPending,
  Reclaimed: styles.badgeReclaimed,
};

interface StatusBadgeProps {
  status: string;
}

export default function StatusBadge({ status }: StatusBadgeProps) {
  return (
    <span
      className={`${styles.badge} ${statusClassMap[status] ?? styles.badgeDraft}`}
    >
      {status}
    </span>
  );
}
