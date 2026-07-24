"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import LanguageSelector from "@/components/LanguageSelector";
import { useTranslation } from "@/context/translation-context";
import { MessageKey } from "@/types/message-key";

const navItems = [
  {
    href: "/dashboard",
    labelKey: MessageKey.Dashboard,
    icon: (
      <path d="M4 13h6V4H4v9Zm0 7h6v-4H4v4Zm10 0h6v-9h-6v9Zm0-16v4h6V4h-6Z" />
    ),
  },
  {
    href: "/goals",
    labelKey: MessageKey.GoalListTitle,
    icon: (
      <>
        <circle cx="12" cy="12" r="8" />
        <circle cx="12" cy="12" r="4" />
        <path d="m14.8 9.2 5.4-5.4M17 3.8h3.2V7" />
      </>
    ),
  },
];

export default function AppShell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  const { t } = useTranslation();

  return (
    <div className="app-shell">
      <aside className="app-sidebar">
        <Link className="brand-lockup" href="/dashboard">
          <span className="brand-orbit" aria-hidden="true">
            <span />
          </span>
          <span>
            <strong>Planner</strong>
            <small>{t(MessageKey.PlanWithClarity)}</small>
          </span>
        </Link>

        <nav className="side-navigation" aria-label={t(MessageKey.Navigation)}>
          {navItems.map((item) => {
            const isActive =
              pathname === item.href || pathname.startsWith(`${item.href}/`);

            return (
              <Link
                key={item.href}
                href={item.href}
                className={`nav-link ${isActive ? "nav-link-active" : ""}`}
              >
                <svg viewBox="0 0 24 24" aria-hidden="true">
                  {item.icon}
                </svg>
                <span>{t(item.labelKey)}</span>
              </Link>
            );
          })}
        </nav>

        <div className="sidebar-footer">
          <LanguageSelector />
        </div>
      </aside>

      <div className="app-stage">
        <header className="mobile-header">
          <Link className="mobile-brand" href="/dashboard">
            <span className="brand-orbit" aria-hidden="true">
              <span />
            </span>
            <strong>Planner</strong>
          </Link>

          <nav aria-label={t(MessageKey.Navigation)}>
            {navItems.map((item) => {
              const isActive =
                pathname === item.href || pathname.startsWith(`${item.href}/`);

              return (
                <Link
                  key={item.href}
                  href={item.href}
                  className={`mobile-nav-link ${
                    isActive ? "mobile-nav-link-active" : ""
                  }`}
                  aria-label={t(item.labelKey)}
                >
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    {item.icon}
                  </svg>
                </Link>
              );
            })}
          </nav>

          <LanguageSelector />
        </header>
        <main className="app-main">{children}</main>
      </div>
    </div>
  );
}
