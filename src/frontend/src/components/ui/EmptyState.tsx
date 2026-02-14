import styles from "@/styles/ui.module.css";

interface EmptyStateProps {
  message: string;
}

export default function EmptyState({ message }: EmptyStateProps) {
  return (
    <div className={styles.emptyContainer}>
      <p>{message}</p>
    </div>
  );
}
