"use client";

import InvoiceList from "@/components/invoices/InvoiceList";
import EmptyState from "@/components/ui/EmptyState";
import ErrorMessage from "@/components/ui/ErrorMessage";
import LoadingSpinner from "@/components/ui/LoadingSpinner";
import Pagination from "@/components/ui/Pagination";
import { useInvoices } from "@/hooks/useInvoices";
import styles from "@/styles/invoices.module.css";
import Link from "next/link";

export default function InvoicesPage() {
  const { data, loading, error, page, setPage, refetch } = useInvoices();

  return (
    <main style={{ maxWidth: "1200px", margin: "0 auto", padding: "2rem" }}>
      <div className={styles.pageHeader}>
        <h1 className={styles.pageTitle}>Invoices</h1>
        <Link href="/invoices/new" className={styles.createButton}>
          New Invoice
        </Link>
      </div>

      {loading && <LoadingSpinner />}
      {error && <ErrorMessage message={error} onRetry={refetch} />}
      {!loading && !error && (!data || data.items.length === 0) && (
        <EmptyState message="No invoices found" />
      )}
      {!loading && !error && data && data.items.length > 0 && (
        <>
          <InvoiceList invoices={data.items} />
          <Pagination
            page={page}
            totalPages={data.totalPages}
            onPageChange={setPage}
          />
        </>
      )}
    </main>
  );
}
