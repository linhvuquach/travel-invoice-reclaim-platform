import type { CreateInvoiceRequest } from "@/types/invoice";
import styles from "@/styles/form.module.css";

interface InvoiceFormProps {
  values: CreateInvoiceRequest;
  errors: Record<string, string>;
  isSubmitting: boolean;
  submitError: string | null;
  onChange: (field: string, value: string | number) => void;
  onSubmit: () => void;
  onReset: () => void;
}

const CURRENCIES = ["EUR", "USD", "GBP"];

export default function InvoiceForm({
  values,
  errors,
  isSubmitting,
  submitError,
  onChange,
  onSubmit,
  onReset,
}: InvoiceFormProps) {
  const handleFormSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit();
  };

  return (
    <div className={styles.formContainer}>
      <form className={styles.form} onSubmit={handleFormSubmit}>
        {submitError && <div className={styles.submitError}>{submitError}</div>}

        <div className={styles.fieldGroup}>
          <label className={`${styles.label} ${styles.required}`}>
            Hotel Name
          </label>
          <input
            className={`${styles.input} ${errors.hotelName ? styles.inputError : ""}`}
            type="text"
            value={values.hotelName}
            onChange={(e) => onChange("hotelName", e.target.value)}
            placeholder="e.g. Grand Hotel Vienna"
          />
          {errors.hotelName && (
            <p className={styles.fieldError}>{errors.hotelName}</p>
          )}
        </div>

        <div className={styles.row}>
          <div className={styles.fieldGroup}>
            <label className={`${styles.label} ${styles.required}`}>
              Invoice Number
            </label>
            <input
              className={`${styles.input} ${errors.invoiceNumber ? styles.inputError : ""}`}
              type="text"
              value={values.invoiceNumber}
              onChange={(e) => onChange("invoiceNumber", e.target.value)}
              placeholder="e.g. INV-2026-001"
            />
            {errors.invoiceNumber && (
              <p className={styles.fieldError}>{errors.invoiceNumber}</p>
            )}
          </div>

          <div className={styles.fieldGroup}>
            <label className={`${styles.label} ${styles.required}`}>
              Issue Date
            </label>
            <input
              className={`${styles.input} ${errors.issueDate ? styles.inputError : ""}`}
              type="date"
              value={values.issueDate}
              onChange={(e) => onChange("issueDate", e.target.value)}
            />
            {errors.issueDate && (
              <p className={styles.fieldError}>{errors.issueDate}</p>
            )}
          </div>
        </div>

        <div className={styles.row}>
          <div className={styles.fieldGroup}>
            <label className={`${styles.label} ${styles.required}`}>
              Total Amount
            </label>
            <input
              className={`${styles.input} ${errors.totalAmount ? styles.inputError : ""}`}
              type="number"
              step="0.01"
              min="0"
              value={values.totalAmount || ""}
              onChange={(e) =>
                onChange("totalAmount", parseFloat(e.target.value) || 0)
              }
              placeholder="0.00"
            />
            {errors.totalAmount && (
              <p className={styles.fieldError}>{errors.totalAmount}</p>
            )}
          </div>

          <div className={styles.fieldGroup}>
            <label className={`${styles.label} ${styles.required}`}>
              VAT Amount
            </label>
            <input
              className={`${styles.input} ${errors.vatAmount ? styles.inputError : ""}`}
              type="number"
              step="0.01"
              min="0"
              value={values.vatAmount || ""}
              onChange={(e) =>
                onChange("vatAmount", parseFloat(e.target.value) || 0)
              }
              placeholder="0.00"
            />
            {errors.vatAmount && (
              <p className={styles.fieldError}>{errors.vatAmount}</p>
            )}
          </div>
        </div>

        <div className={styles.fieldGroup}>
          <label className={`${styles.label} ${styles.required}`}>
            Currency
          </label>
          <select
            className={`${styles.select} ${errors.currency ? styles.inputError : ""}`}
            value={values.currency}
            onChange={(e) => onChange("currency", e.target.value)}
          >
            {CURRENCIES.map((c) => (
              <option key={c} value={c}>
                {c}
              </option>
            ))}
          </select>
          {errors.currency && (
            <p className={styles.fieldError}>{errors.currency}</p>
          )}
        </div>

        <div className={styles.fieldGroup}>
          <label className={styles.label}>Description</label>
          <textarea
            className={styles.textarea}
            value={values.description ?? ""}
            onChange={(e) => onChange("description", e.target.value)}
            placeholder="Optional notes about this invoice..."
          />
        </div>

        <div className={styles.actions}>
          <button
            type="submit"
            className={styles.submitButton}
            disabled={isSubmitting}
          >
            {isSubmitting ? "Submitting..." : "Submit Invoice"}
          </button>
          <button
            type="button"
            className={styles.resetButton}
            onClick={onReset}
            disabled={isSubmitting}
          >
            Reset
          </button>
        </div>
      </form>
    </div>
  );
}
