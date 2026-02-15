import styles from "@/styles/detail.module.css";

interface DetailFieldProps {
  label: string;
  value: string;
  variant?: "default" | "amount";
  fullWidth?: boolean;
}

export default function DetailField({
  label,
  value,
  variant = "default",
  fullWidth = false,
}: DetailFieldProps) {
  return (
    <div className={`${styles.field} ${fullWidth ? styles.description : ""}`}>
      <span className={styles.fieldLabel}>{label}</span>
      <span
        className={
          variant === "amount" ? styles.amountValue : styles.fieldValue
        }
      >
        {value}
      </span>
    </div>
  );
}
