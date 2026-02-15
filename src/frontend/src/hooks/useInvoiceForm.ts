import { createInvoice } from "@/lib/api/invoices";
import { CreateInvoiceRequest } from "@/types/invoice";
import { useRouter } from "next/router";
import { useCallback, useReducer } from "react";

type FormState = {
  values: CreateInvoiceRequest;
  errors: Record<string, string>;
  isSubmitting: boolean;
  submitError: string | null;
};

type FormAction =
  | { type: "SET_FIELD"; field: string; value: string | number }
  | { type: "SET_ERRORS"; errors: Record<string, string> }
  | { type: "SUBMIT_START" }
  | { type: "SUBMIT_SUCCESS" }
  | { type: "SUBMIT_ERROR"; error: string }
  | { type: "RESET" };

const initialValues: CreateInvoiceRequest = {
  hotelName: "",
  invoiceNumber: "",
  issueDate: "",
  totalAmount: 0,
  vatAmount: 0,
  currency: "EUR",
  description: "",
};

const initialState: FormState = {
  values: initialValues,
  errors: {},
  isSubmitting: false,
  submitError: null,
};

function formReducer(state: FormState, action: FormAction): FormState {
  switch (action.type) {
    case "SET_FIELD":
      return {
        ...state,
        values: { ...state.values, [action.field]: action.value },
        errors: { ...state.errors, [action.field]: "" },
        submitError: null,
      };
    case "SET_ERRORS":
      return { ...state, errors: action.errors };
    case "SUBMIT_START":
      return { ...state, isSubmitting: true, submitError: null };
    case "SUBMIT_SUCCESS":
      return { ...initialState };
    case "SUBMIT_ERROR":
      return { ...state, isSubmitting: false, submitError: action.error };
    case "RESET":
      return { ...initialState };
  }
}

function validate(values: CreateInvoiceRequest): Record<string, string> {
  const errors: Record<string, string> = {};

  if (!values.hotelName.trim()) errors.hotelName = "Hotel name is required";
  if (!values.invoiceNumber.trim())
    errors.invoiceNumber = "Invoice number is required";
  if (!values.issueDate) errors.issueDate = "Issue date is required";
  if (values.totalAmount <= 0)
    errors.totalAmount = "Total amount must be greater than 0";
  if (values.vatAmount < 0) errors.vatAmount = "VAT amount cannot be negative";
  if (values.vatAmount > values.totalAmount)
    errors.vatAmount = "VAT amount cannot exceed total amount";
  if (!values.currency) errors.currency = "Currency is required";

  return errors;
}

export function useInvoiceFrom() {
  const [state, dispatch] = useReducer(formReducer, initialState);
  const router = useRouter();

  const handleChange = useCallback((field: string, value: string | number) => {
    dispatch({ type: "SET_FIELD", field, value });
  }, []);

  const handleSubmit = useCallback(async () => {
    const errors = validate(state.values);
    if (Object.keys(errors).length > 0) {
      dispatch({ type: "SET_ERRORS", errors });
      return;
    }

    dispatch({ type: "SUBMIT_START" });

    try {
      await createInvoice(state.values);
      dispatch({ type: "SUBMIT_SUCCESS" });
      router.push("/invoices");
    } catch (error) {
      const message =
        error instanceof Error ? error.message : "Failed to create invoice";
      dispatch({ type: "SUBMIT_ERROR", error: message });
    }
  }, [state.values, router]);

  const resetForm = useCallback(() => {
    dispatch({ type: "RESET" });
  }, []);

  return { state, handleChange, handleSubmit, resetForm };
}
