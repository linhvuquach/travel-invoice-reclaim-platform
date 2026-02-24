"use client";

import ErrorMessage from "@/components/ui/ErrorMessage";
import LoadingSpinner from "@/components/ui/LoadingSpinner";
import { useReclaim } from "@/hooks/useReclaim";
import Link from "next/link";
import { use } from "react";
import styles from "@/styles/detail.module.css";
import ReclaimStatusBadge from "@/components/reclaims/ReclaimStatusBadge";
import ReclaimStatusCard from "@/components/reclaims/ReclaimStatusCard";
import ReclaimActions from "@/components/reclaims/ReclaimActions";

export default function ReclaimDetailPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = use(params);
  const { reclaim, loading, error } = useReclaim(id);

  if (loading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message={error} />;
  if (!reclaim) return <ErrorMessage message="Reclaim not found" />;

  return (
    <main style={{ maxWidth: "800px", margin: "0 auto", padding: "2rem" }}>
      <Link href="/reclaims" className={styles.backLink}>
        &larr; Back to Reclaims
      </Link>

      <div style={{ display: "flex", alignItems: "center", gap: "1rem" }}>
        <h1 className={styles.title}>{reclaim.hotelName}</h1>
        <ReclaimStatusBadge status={reclaim.status} />
      </div>

      <ReclaimStatusCard reclaim={reclaim} />
      <ReclaimActions reclaimId={reclaim.id} status={reclaim.status} />
    </main>
  );
}
