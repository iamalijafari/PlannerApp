import { API_BASE_URL } from "@/config/api";
import { ResponseModel } from "@/types/response-model";

export async function apiRequest<T>(
  path: string,
  init: RequestInit = {},
): Promise<ResponseModel<T>> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    cache: "no-store",
    ...init,
    headers: {
      "Content-Type": "application/json",
      ...init.headers,
    },
  });

  if (!response.ok) {
    throw new Error(`Request failed with status ${response.status}`);
  }

  return response.json() as Promise<ResponseModel<T>>;
}
