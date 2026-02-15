import type { Payment } from "@/types/invoice";
import DetailField from "@/components/ui/DetailField";
import styles from "@/styles/detail.module.css";
import { formatCurrency, formatDate } from "@/utils";

interface PaymentDetailProps {
  payment: Payment | undefined;
  currency: string;
}

export default function PaymentDetail({
  payment,
  currency,
}: PaymentDetailProps) {
  return (
    <div className={styles.paymentSection}>
      <h2 className={styles.sectionTitle}>Payment</h2>
      {payment ? (
        <div className={styles.detailGrid}>
          <DetailField
            label="Amount"
            value={formatCurrency(payment.amount, currency)}
            variant="amount"
          />
          <DetailField label="Method" value={payment.method} />
          <DetailField
            label="Payment Date"
            value={formatDate(payment.paymentDate)}
          />
          <DetailField label="Status" value={payment.status} />
        </div>
      ) : (
        <p className={styles.noPayment}>No payment recorded yet</p>
      )}
    </div>
  );
}
