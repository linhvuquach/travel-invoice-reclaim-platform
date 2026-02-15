import InvoiceForm from "@/components/invoices/InvoiceForm";
import { useInvoiceFrom } from "@/hooks/useInvoiceForm";
import Link from "next/link";

export default function NewInvoicePage() {
  const { state, handleChange, handleSubmit, resetForm } = useInvoiceFrom();

  return (
    <main style={{ maxWidth: "1200px", margin: "0 auto", padding: "2rem" }}>
      <div style={{ marginBottom: "1.5rem" }}>
        <Link
          href="/invoices"
          style={{ color: "#3b82f6", fontSize: "0.875rem" }}
        >
          &larr; Back to Invoices
        </Link>
        <h1
          style={{ fontSize: "1.5rem", fontWeight: 700, marginTop: "0.5rem" }}
        >
          New Invoice
        </h1>
      </div>

      <InvoiceForm
        values={state.values}
        errors={state.errors}
        isSubmitting={state.isSubmitting}
        submitError={state.submitError}
        onChange={handleChange}
        onSubmit={handleSubmit}
        onReset={resetForm}
      />
    </main>
  );
}
