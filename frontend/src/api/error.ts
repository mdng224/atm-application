export type ApiErrorResponse = {
  code?: string;
  message?: string; // custom error shape
  detail?: string; // RFC 7807 ProblemDetails.detail
  title?: string; // RFC 7807 ProblemDetails.title
  error?: string; // some libs use this
  errors?: Record<string, string[] | string>; // ValidationProblemDetails
};

export function extractApiError(err: unknown, preferredField?: string): string {
  const anyErr = err as any;
  const status: number | undefined = anyErr?.response?.status;
  const data: ApiErrorResponse | undefined = anyErr?.response?.data;

  // 1) Field-specific validation (backend-driven text)
  if (preferredField && data?.errors && data.errors[preferredField]) {
    const val = data.errors[preferredField];
    if (Array.isArray(val) && val.length) return val[0];
    if (typeof val === 'string' && val.trim()) return val;
  }

  // 2) Any validation error (backend-driven text)
  if (data?.errors) {
    const firstKey = Object.keys(data.errors)[0];
    const val = (data.errors as any)[firstKey];
    if (Array.isArray(val) && val.length) return val[0];
    if (typeof val === 'string' && val.trim()) return val;
  }

  // 3) Backend-provided problem details / custom fields
  if (data?.detail && data.detail.trim()) return data.detail;
  if (data?.message && data.message.trim()) return data.message;
  if (data?.title && data.title.trim()) return data.title;
  if (data?.error && typeof data.error === 'string' && data.error.trim()) return data.error;

  // 4) Optional: expose code if you want *some* backend info
  if (data?.code && data.code.trim()) {
    return data.code;
  }

  // 5) Status-based super-generic fallbacks (only when backend gave nothing)
  if (status === 400) return 'Bad request.';
  if (status === 404) return 'Not found.';
  if (status === 409) return 'Request could not be completed due to a conflict.';
  if (status === 500) return 'Server error. Please try again.';

  // 6) Axios/network-level error
  if (typeof anyErr?.message === 'string' && anyErr.message.trim()) return anyErr.message;

  return 'An unexpected error occurred.';
}
