import "./globals.css";
import { LanguageProvider } from "@/context/languageContext";
import { TranslationProvider } from "@/context/translationContext";
import LanguageSelector from "@/components/LanguageSelector";

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <LanguageProvider>
          <TranslationProvider>
            <LanguageSelector />
            {children}
          </TranslationProvider>
        </LanguageProvider>
      </body>
    </html>
  );
}