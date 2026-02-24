import { Reclaim } from "@/types/reclaim";
import Link from "next/link";
import ReclaimStatusBadge from "./ReclaimStatusBadge";
import styles from "@/styles/reclaims.module.css";
import { formatCurrency, formatDate } from "@/utils";

interface ReclaimListProps {
  reclaims: Reclaim[];
}

export default function ReclaimList({ reclaims }: ReclaimListProps) {
  return (
    <div className={styles.reclaimGrid}>
      {reclaims.map((reclaim) => (
        <Link
          key={reclaim.id}
          href={`/reclaims/${reclaim.id}`}
          className={styles.reclaimCard}
        >
          <div className={styles.cardHeader}>
            <div>
              <h3 className={styles.hotelName}>{reclaim.hotelName}</h3>
              <p className={styles.invoiceNumber}>#{reclaim.invoiceNumber}</p>
            </div>
            <ReclaimStatusBadge status={reclaim.status} />
          </div>
          <div className={styles.cardBody}>
            <span className={styles.amount}>
              {formatCurrency(reclaim.eligibleAmount, "EUR")}
            </span>
            <span className={styles.date}>
              Created {formatDate(reclaim.createdDate)}
            </span>
          </div>
        </Link>
      ))}
    </div>
  );
}
