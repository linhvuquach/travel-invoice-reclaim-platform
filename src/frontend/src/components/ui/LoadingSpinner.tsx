import styles from "@/styles/ui.module.css";

export default function LoadingSpinner() {
  return (
    <div className={styles.spinerContainer}>
      <div className={styles.spinner} />
      <p>Loading...</p>
    </div>
  );
}
