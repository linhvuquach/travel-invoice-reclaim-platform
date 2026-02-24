"use client";

import ReclaimList from "@/components/reclaims/ReclaimList";
import EmptyState from "@/components/ui/EmptyState";
import ErrorMessage from "@/components/ui/ErrorMessage";
import LoadingSpinner from "@/components/ui/LoadingSpinner";
import Pagination from "@/components/ui/Pagination";
import { useReclaims } from "@/hooks/useReclaims";
import styles from "@/styles/reclaims.module.css";
import Link from "next/link";

const STATUS_FILTERS = ["All", "Pending", "Approved", "Rejected", "Paid"];

export default function ReclaimsPage() {
  const {
    data,
    loading,
    error,
    page,
    setPage,
    statusFilter,
    setStatusFilter,
    refetch,
  } = useReclaims();

  return (
    <main style={{ maxWidth: "1200px", margin: "0 auto", padding: "2rem" }}>
      <div className={styles.pageHeader}>
        <h1 className={styles.pageTitle}>Reclaims</h1>
        <Link
          href="/invoices"
          style={{
            color: "#3b82f6",
            fontSize: "0.875rem",
            textDecoration: "none",
          }}
        >
          View Invoices
        </Link>
      </div>

      <div className={styles.filterBar}>
        {STATUS_FILTERS.map((s) => {
          const filterValue = s === "All" ? undefined : s;
          const isActive = statusFilter === filterValue;
          return (
            <button
              key={s}
              className={`${styles.filterButton} ${isActive ? styles.filterButtonActive : ""}`}
              onClick={() => {
                setStatusFilter(filterValue);
                setPage(1);
              }}
            >
              {s}
            </button>
          );
        })}
      </div>

      {loading && <LoadingSpinner />}
      {error && <ErrorMessage message={error} onRetry={refetch} />}
      {!loading && !error && (!data || data.items.length === 0) && (
        <EmptyState message="No reclaims found" />
      )}
      {!loading && !error && data && data.items.length > 0 && (
        <>
          <ReclaimList reclaims={data.items} />
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
