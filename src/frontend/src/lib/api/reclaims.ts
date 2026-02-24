import {
  PagedReclaimResponse,
  ProcessReclaimResponse,
  Reclaim,
  ReclaimQueryParams,
} from "@/types/reclaim";
import { apiClient } from "./client";

export async function processReclaim(
  invoiceId: string,
): Promise<ProcessReclaimResponse> {
  const { data } = await apiClient.post<ProcessReclaimResponse>(
    `/reclaims/${invoiceId}/process`,
  );
  return data;
}

export async function getReclaims(
  params: ReclaimQueryParams = {},
): Promise<PagedReclaimResponse> {
  const { data } = await apiClient.get<PagedReclaimResponse>("/reclaims", {
    params,
  });
  return data;
}

export async function getReclaimById(id: string): Promise<Reclaim> {
  const { data } = await apiClient.get<Reclaim>(`/reclaims/${id}`);
  return data;
}

export async function approveReclaim(id: string): Promise<Reclaim> {
  const { data } = await apiClient.post<Reclaim>(`/reclaims/${id}/approve`);
  return data;
}

export async function rejectReclaim(
  id: string,
  reason: string,
): Promise<Reclaim> {
  const { data } = await apiClient.post<Reclaim>(`/reclaims/${id}/reject`, {
    reason,
  });
  return data;
}
