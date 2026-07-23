import { API_BASE_URL } from "@/config/api";
import { Language } from "@/types/language";
import { MessageKey } from "@/types/message-key";

const cache = new Map<string, string>();

export async function translate(
  messageKey: MessageKey,
  language: Language,
): Promise<string> {
  const cacheKey = `${language}:${messageKey}`;
  const cachedValue = cache.get(cacheKey);
  if (cachedValue) return cachedValue;

  try {
    const response = await fetch(`${API_BASE_URL}/translation/translate`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ messageKey, language }),
    });

    if (!response.ok) {
      throw new Error(`Translation request failed with ${response.status}`);
    }

    const value = await response.text();
    cache.set(cacheKey, value);
    return value;
  } catch (error) {
    console.error("Failed to load translation:", error);
    return MessageKey[messageKey];
  }
}
