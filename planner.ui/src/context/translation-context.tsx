"use client";

import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";
import { useLanguage } from "@/context/language-context";
import { translate } from "@/services/translation-service";
import { Language } from "@/types/language";
import { MessageKey } from "@/types/message-key";

interface TranslationContextType {
  t: (key: MessageKey, values?: Record<string, string | number>) => string;
  isLoading: boolean;
}

const TranslationContext = createContext<TranslationContextType | undefined>(undefined);
const translationCache = new Map<Language, Record<number, string>>();

export const TranslationProvider = ({ children }: { children: ReactNode }) => {
  const { language } = useLanguage();
  const [translations, setTranslations] = useState<Record<number, string>>(
    () => translationCache.get(language) ?? {},
  );
  const [isLoading, setIsLoading] = useState(
    () => !translationCache.has(language),
  );

  useEffect(() => {
    let isCurrent = true;

    const loadTranslations = async () => {
      const cachedTranslations = translationCache.get(language);
      if (cachedTranslations) {
        setTranslations(cachedTranslations);
        setIsLoading(false);
        return;
      }

      setIsLoading(true);
      const keys = Object.values(MessageKey).filter(
        (value): value is MessageKey => typeof value === "number",
      );
      const entries = await Promise.all(
        keys.map(async (key) => {
          const value = await translate(key, language);
          return [key, value] as const;
        }),
      );

      const loadedTranslations = Object.fromEntries(entries);
      translationCache.set(language, loadedTranslations);

      if (isCurrent) {
        setTranslations(loadedTranslations);
        setIsLoading(false);
      }
    };

    void loadTranslations();

    return () => {
      isCurrent = false;
    };
  }, [language]);

  const t = useCallback(
    (key: MessageKey, values: Record<string, string | number> = {}): string => {
      const template = translations[key] ?? MessageKey[key];
      return Object.entries(values).reduce(
        (message, [name, value]) =>
          message.replaceAll(`{${name}}`, String(value)),
        template,
      );
    },
    [translations],
  );

  const value = useMemo(
    () => ({ t, isLoading }),
    [isLoading, t],
  );

  return (
    <TranslationContext.Provider value={value}>
      {children}
    </TranslationContext.Provider>
  );
};

export const useTranslation = () => {
  const ctx = useContext(TranslationContext);
  if (!ctx) throw new Error("useTranslation must be used within TranslationProvider");
  return ctx;
};
