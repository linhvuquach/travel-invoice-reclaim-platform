"use client";

import { use } from "react";
import Link from "next/link";
import { useInvoice } from "@/hooks/useInvoice";
import InvoiceDetail from "@/components/invoices/InvoiceDetail";
import PaymentDetail from "@/components/invoices/PaymentDetail";
import StatusBadge from "@/components/ui/StatusBadge";
import LoadingSpinner from "@/components/ui/LoadingSpinner";
import ErrorMessage from "@/components/ui/ErrorMessage";
import styles from "@/styles/detail.module.css";

export default function InvoiceDetailPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = use(params);
  const { invoice, loading, error } = useInvoice(id);

  if (loading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message={error} />;
  if (!invoice) return <ErrorMessage message="Invoice not found" />;

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <Link href="/invoices" className={styles.backLink}>
        &larr; Back to Invoices
      </Link>

      <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
        <h1 className={styles.title}>{invoice.hotelName}</h1>
        <StatusBadge status={invoice.status} />
      </div>

      <InvoiceDetail invoice={invoice} />
      <PaymentDetail payment={invoice.payment} currency={invoice.currency} />
    </main>
  );
}
