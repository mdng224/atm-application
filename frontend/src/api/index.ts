// src/api/index.ts
import type { AxiosInstance } from 'axios';
import axios from 'axios';

/**
 * Axios client preconfigured for your backend API.
 *
 * - Sets baseURL from VITE_API_BASE_URL or defaults to localhost.
 * - Uses JSON content.
 * - No auth / token logic (ATM app is single-user + anonymous).
 */
const apiClient: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000',
  headers: { 'Content-Type': 'application/json' },
  withCredentials: false,
});

/* -------------------------------------------------------------------------- */
/*                              Request Interceptor                            */
/* -------------------------------------------------------------------------- */

// No auth headers required, but keep the interceptor for future extensibility
apiClient.interceptors.request.use(config => {
  // You can modify headers here if needed later
  return config;
});

/* -------------------------------------------------------------------------- */
/*                              Response Interceptor                           */
/* -------------------------------------------------------------------------- */

apiClient.interceptors.response.use(
  response => response,
  err => {
    // No auth clearing logic necessary
    return Promise.reject(err);
  },
);

export default apiClient;
