export interface Payment {
  id: string;
  amount: number;
  paymentDate: string;
  method: string;
  status: string;
}

export interface Invoice {
  id: string;
  hotelName: string;
  invoiceNumber: string;
  issueDate: string;
  submissionDate: string;
  totalAmount: number;
  vatAmount: number;
  currency: string;
  status: string;
  description?: string;
  payment?: Payment;
}

export interface PagedResponse<T> {
    items: T[];
    page: number;
    totalCount: number;
    totalPage: number;
}

export interface CreateInvoiceRequest {
  hotelName: string;
  invoiceNumber: string;
  issueDate: string;
  totalAmount: number;
  vatAmount: number;
  currency: string;
  description?: string;
}

export interface InvoiceQueryParams {
  page?: number;
  pageSize?: number;
  hotelName?: string;
  status?: string;
  fromDate?: string;
  toDate?: string;
}
