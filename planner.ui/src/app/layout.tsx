import "./globals.css";
import type { Metadata } from "next";
import AppShell from "@/components/AppShell";
import { LanguageProvider } from "@/context/language-context";
import { TranslationProvider } from "@/context/translation-context";

export const metadata: Metadata = {
  title: {
    default: "PlannerApp",
    template: "%s | PlannerApp",
  },
  description:
    "Turn long-term goals into measurable yearly, monthly, weekly, and daily progress.",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="fa" dir="rtl" className="dark" suppressHydrationWarning>
      <body>
        <LanguageProvider>
          <TranslationProvider>
            <AppShell>{children}</AppShell>
          </TranslationProvider>
        </LanguageProvider>
      </body>
    </html>
  );
}
