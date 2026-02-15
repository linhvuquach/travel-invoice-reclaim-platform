import type { Invoice } from "@/types/invoice";
import DetailField from "@/components/ui/DetailField";
import styles from "@/styles/detail.module.css";
import { formatCurrency, formatDate } from "@/utils";

interface InvoiceDetailProps {
  invoice: Invoice;
}

export default function InvoiceDetail({ invoice }: InvoiceDetailProps) {
  return (
    <div className={styles.detailCard}>
      <div className={styles.detailGrid}>
        <DetailField
          label="Invoice Number"
          value={`#${invoice.invoiceNumber}`}
        />
        <DetailField label="Issue Date" value={formatDate(invoice.issueDate)} />
        <DetailField
          label="Total Amount"
          value={formatCurrency(invoice.totalAmount, invoice.currency)}
          variant="amount"
        />
        <DetailField
          label="VAT Amount"
          value={formatCurrency(invoice.vatAmount, invoice.currency)}
          variant="amount"
        />
        <DetailField label="Currency" value={invoice.currency} />
        <DetailField
          label="Submitted"
          value={formatDate(invoice.submissionDate)}
        />
        {invoice.description && (
          <DetailField
            label="Description"
            value={invoice.description}
            fullWidth
          />
        )}
      </div>
    </div>
  );
}
