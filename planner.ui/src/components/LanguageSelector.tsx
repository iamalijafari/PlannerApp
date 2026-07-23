"use client";

import { Language } from "@/types/language";
import { useLanguage } from "@/context/language-context";
import { useTranslation } from "@/context/translation-context";
import { MessageKey } from "@/types/message-key";

export default function LanguageSelector() {
  const { language, setLanguage } = useLanguage();
  const { t } = useTranslation();

  const options = [
    { label: "فارسی", value: Language.fa },
    { label: "English", value: Language.en },
  ];

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = Number(e.target.value);

    setLanguage(value as Language);
  };

  return (
    <label className="language-selector">
      <span className="sr-only">{t(MessageKey.Language)}</span>
      <select
        aria-label={t(MessageKey.Language)}
        value={language}
        onChange={handleChange}
        className="rounded-lg border border-zinc-300 bg-white px-3 py-2 text-sm shadow-sm dark:border-zinc-700 dark:bg-zinc-950"
      >
        {options.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
    </label>
  );
}
