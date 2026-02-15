import { Invoice } from "@/types/invoice";
import Link from "next/link";
import styles from "@/styles/invoices.module.css";
import { formatCurrency, formatDate } from "@/utils";
import StatusBadge from "../ui/StatusBadge";

interface InvoiceListProps {
  invoices: Invoice[];
}

export default function InvoiceList({ invoices }: InvoiceListProps) {
  return (
    <div className={styles.invoiceGrid}>
      {invoices.map((invoice) => (
        <Link
          key={invoice.id}
          href={`/invoices/${invoice.id}`}
          className={styles.invoiceCard}
        >
          <div className={styles.cardHeader}>
            <div>
              <h3 className={styles.hotelName}>{invoice.hotelName}</h3>
              <p className={styles.invoiceNumber}>#{invoice.invoiceNumber}</p>
            </div>
            <StatusBadge status={invoice.status} />
          </div>
          <div className={styles.cardBody}>
            <span className={styles.amount}>
              {formatCurrency(invoice.totalAmount, invoice.currency)}
            </span>
            <span className={styles.date}>
              Submitted {formatDate(invoice.submissionDate)}
            </span>
          </div>
        </Link>
      ))}
    </div>
  );
}
