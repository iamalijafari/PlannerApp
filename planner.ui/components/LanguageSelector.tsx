"use client";

import { Language } from "@/types/language";
import { useLanguage } from "@/context/languageContext";

export default function LanguageSelector() {
  const { language, setLanguage } = useLanguage();

  const options = [
    { label: "فارسی", value: Language.fa },
    { label: "English", value: Language.en },
  ];

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const value = Number(e.target.value);

    setLanguage(value as Language);
  };

  return (
    <select
      value={language}
      onChange={handleChange}
      className="border px-2 py-1 rounded"
    >
      {options.map((opt) => (
        <option key={opt.value} value={opt.value}>
          {opt.label}
        </option>
      ))}
    </select>
  );
}