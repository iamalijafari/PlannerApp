import "./globals.css";
import type { Metadata } from "next";
import Link from "next/link";
import LanguageSelector from "@/components/LanguageSelector";
import { LanguageProvider } from "@/context/language-context";
import { TranslationProvider } from "@/context/translation-context";

export const metadata: Metadata = {
  title: "Planner",
  description: "Plan and organize goals from yearly milestones to daily tasks.",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="fa" dir="rtl" suppressHydrationWarning>
      <body>
        <LanguageProvider>
          <TranslationProvider>
            <header className="app-header">
              <Link className="app-brand" href="/goals">
                Planner
              </Link>
              <LanguageSelector />
            </header>
            <main>{children}</main>
          </TranslationProvider>
        </LanguageProvider>
      </body>
    </html>
  );
}
