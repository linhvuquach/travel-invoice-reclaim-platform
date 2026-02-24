import type { PagedResponse } from "./invoice";

export interface Reclaim {
  id: string;
  invoiceId: string;
  invoiceNumber: string;
  hotelName: string;
  eligibleAmount: number;
  status: string;
  createdDate: string;
  processedDate?: string;
  rejectionReason?: string;
  processedBy?: string;
}

export interface ValidationRuleResult {
  ruleName: string;
  passed: boolean;
  failureReason?: string;
}

export interface ProcessReclaimResponse {
  reclaim?: Reclaim;
  isValid: boolean;
  validationResults: ValidationRuleResult[];
}

export interface ReclaimQueryParams {
  page?: number;
  pageSize?: number;
  status?: string;
}

export type PagedReclaimResponse = PagedResponse<Reclaim>;
