import axios from "axios";

export const apiClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5000/api/v1",
  headers: { "Content-Type": "application/json" },
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // Transform Axios errors to a consistent shape matching RFC 7807 ProblemDetails
    const message =
      error.response?.data?.detail ??
      error.response?.data?.title ??
      error.message ??
      "An unexpected error occurred";

    return Promise.reject(new Error(message));
  },
);
