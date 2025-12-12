import { TranslationRequestModel } from "@/types/translation-request-model";
import { getEnv } from "@/src/config/env";

const API_BASE = getEnv("NEXT_PUBLIC_TRANSLATION_API_URL");

export async function translateApi(model: TranslationRequestModel): Promise<string> {
  const response = await fetch(API_BASE, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(model)
  });

  if (!response.ok) {
    throw new Error("Translation API error");
  }

  return response.text();
}