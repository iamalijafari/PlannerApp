"use client";

import { useTranslation } from "@/context/translationContext";
import { ResponseModel } from "@/types/response-model";

export function useApiResponse() {
  const { t } = useTranslation();

  const handleResponse = async <T>(
    response: ResponseModel<T>,
    onSuccess: (data: T) => void,
    onError: (message: string) => void
  ) => {
    if (response.success) {
      onSuccess(response.result);
      return;
    }

    const translation = t(response.messageKey);
    onError(translation);
  };

  return { handleResponse };
}