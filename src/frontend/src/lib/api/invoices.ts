import {
  CreateInvoiceRequest,
  Invoice,
  InvoiceQueryParams,
  PagedResponse,
} from "@/types/invoice";
import { apiClient } from "./client";

export async function getInvoices(
  params: InvoiceQueryParams = {},
): Promise<PagedResponse<Invoice>> {
  const { data } = await apiClient.get<PagedResponse<Invoice>>("/invoices", {
    params,
  });

  return data;
}

export async function getInvoiceByid(id: string): Promise<Invoice> {
  const { data } = await apiClient.get<Invoice>(`/invoices${id}`);

  return data;
}

export async function createInvoice(
  request: CreateInvoiceRequest,
): Promise<Invoice> {
  const { data } = await apiClient.post<Invoice>("invoices", request);

  return data;
}
