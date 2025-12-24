const cache = new Map<string, string>();

export async function t(key: string, language = 'English'): Promise<string> {
  const cacheKey = `${language}:${key}`;
  if (cache.has(cacheKey)) return cache.get(cacheKey)!;
  try {
    const apiUrl = process.env.NEXT_PUBLIC_TRANSLATION_API_URL || '/api/translation/translate';
    const res = await fetch(apiUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ MessageKey: key, Language: language }),
    });
    if (!res.ok) return key;
    const text = await res.text();
    cache.set(cacheKey, text);
    return text;
  } catch (e) {
    console.error('translation error', e);
    return key;
  }
}

export default { t };

export async function translateApi(model: any): Promise<string> {
  try {
    const apiUrl = process.env.NEXT_PUBLIC_TRANSLATION_API_URL || '/api/translation/translate';
    const res = await fetch(apiUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(model),
    });
    if (!res.ok) throw new Error('Translation API failed');
    return await res.text();
  } catch (e) {
    console.error('translateApi error', e);
    return String(model.MessageKey);
  }
}
