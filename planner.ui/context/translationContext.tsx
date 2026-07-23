"use client";

import { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { MessageKey } from "@/types/message-key";
import { useLanguage } from "@/context/languageContext";
import { translateApi } from "@/src/services/translationService";
import { TranslationRequestModel } from "@/types/translation-request-model";

interface TranslationContextType {
  t: (key: MessageKey) => string;
}

const TranslationContext = createContext<TranslationContextType | undefined>(undefined);

export const TranslationProvider = ({ children }: { children: ReactNode }) => {
  const { language } = useLanguage();

  const [translations, setTranslations] = useState<Record<number, string>>({});

  useEffect(() => {
    const loadTranslations = async () => {
      const keys = Object.values(MessageKey).filter(
        v => typeof v === "number"
      ) as number[];

      const entries = await Promise.all(
        keys.map(async (key) => {
          const model: TranslationRequestModel = {
            MessageKey: key as MessageKey,
            Language: language,
          };

          const value = await translateApi(model);
          return [key, value] as const;
        })
      );

      setTranslations(Object.fromEntries(entries));
    };

    loadTranslations();
  }, [language]);

  const t = (key: MessageKey): string => {
    const numericKey = key as number;
    return translations[numericKey] ?? MessageKey[key];
  };

  return (
    <TranslationContext.Provider value={{ t }}>
      {children}
    </TranslationContext.Provider>
  );
};

export const useTranslation = () => {
  const ctx = useContext(TranslationContext);
  if (!ctx) throw new Error("useTranslation must be used within TranslationProvider");
  return ctx;
};