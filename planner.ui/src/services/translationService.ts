import { TranslationRequestModel } from "@/types/translation-request-model";

export async function translateApi(
  model: TranslationRequestModel
): Promise<string> {
  const apiUrl = process.env.NEXT_PUBLIC_TRANSLATION_API_URL;

  if (!apiUrl) {
    throw new Error("NEXT_PUBLIC_TRANSLATION_API_URL is missing");
  }

  const response = await fetch(apiUrl, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(model),
  });

  if (!response.ok) {
    console.error("Translation API error:", response.status, response.statusText);
    throw new Error("Translation request failed");
  }

  return await response.text();
}