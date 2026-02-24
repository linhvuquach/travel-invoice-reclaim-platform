import styles from "@/styles/reclaims.module.css";

const statusClassMap: Record<string, string> = {
  Pending: styles.badgePending,
  Approved: styles.badgeApproved,
  Rejected: styles.badgeRejected,
  Paid: styles.badgePaid,
};

interface ReclaimStatusBadgeProps {
  status: string;
}

export default function ReclaimStatusBadge({
  status,
}: ReclaimStatusBadgeProps) {
  return (
    <span
      className={`${styles.reclaimBadge} ${statusClassMap[status] ?? styles.badgePending}`}
    >
      {status}
    </span>
  );
}
