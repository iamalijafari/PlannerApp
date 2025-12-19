"use client";

import React from "react";
import { useLanguage } from "@/context/languageContext";
import { Language } from "@/types/language";

interface DatePickerProps {
  value: string; // ISO date YYYY-MM-DD
  onChange: (isoDate: string) => void;
}

// Jalali (Persian) conversion utilities
// Algorithms adapted from public domain conversion methods
function div(a: number, b: number) {
  return Math.floor(a / b);
}

function gregorianToJalali(gy: number, gm: number, gd: number): [number, number, number] {
  const g_d_m = [0,31,59,90,120,151,181,212,243,273,304,334];
  const gy2 = gy - 1600;
  const gm2 = gm - 1;
  const gd2 = gd - 1;

  let g_day_no = 365 * gy2 + div((gy2 + 3), 4) - div((gy2 + 99), 100) + div((gy2 + 399), 400);
  g_day_no += g_d_m[gm2] + gd2;
  if (gm2 > 1 && ((gy % 4 === 0 && gy % 100 !== 0) || (gy % 400 === 0))) g_day_no++;

  let j_day_no = g_day_no - 79;

  const j_np = div(j_day_no, 12053);
  j_day_no = j_day_no % 12053;

  let jy = 979 + 33 * j_np + 4 * div(j_day_no, 1461);
  j_day_no %= 1461;

  if (j_day_no >= 366) {
    jy += div((j_day_no - 366), 365);
    j_day_no = (j_day_no - 366) % 365;
  }

  const jm = j_day_no < 186 ? 1 + div(j_day_no, 31) : 7 + div((j_day_no - 186), 30);
  const jd = 1 + (j_day_no < 186 ? (j_day_no % 31) : ((j_day_no - 186) % 30));

  return [jy, jm, jd];
}

function jalaliToGregorian(jy: number, jm: number, jd: number): [number, number, number] {
  const jy2 = jy - 979;
  const jm2 = jm - 1;
  const jd2 = jd - 1;

  let j_day_no = 365 * jy2 + div(jy2, 33) * 8 + div((jy2 % 33 + 3), 4);
  for (let i = 0; i < jm2; ++i) j_day_no += i < 6 ? 31 : 30;
  j_day_no += jd2;

  let g_day_no = j_day_no + 79;

  let gy = 1600 + 400 * div(g_day_no, 146097);
  g_day_no = g_day_no % 146097;

  let leap = true;
  if (g_day_no >= 36525) {
    g_day_no -= 1;
    gy += 100 * div(g_day_no, 36524);
    g_day_no = g_day_no % 36524;
    if (g_day_no >= 365) g_day_no += 1; else leap = false;
  }

  gy += 4 * div(g_day_no, 1461);
  g_day_no %= 1461;

  if (g_day_no >= 366) {
    leap = false;
    g_day_no -= 1;
    gy += div(g_day_no, 365);
    g_day_no = g_day_no % 365;
  }

  const gd_m = [31, (leap ? 29 : 28), 31,30,31,30,31,31,30,31,30,31];
  let gm = 0;
  for (let i = 0; i < 12; i++) {
    if (g_day_no < gd_m[i]) { gm = i + 1; break; }
    g_day_no -= gd_m[i];
  }
  const gd = g_day_no + 1;

  return [gy, gm, gd];
}

function isoToParts(iso: string): [number, number, number] {
  if (!iso) {
    const d = new Date();
    return [d.getFullYear(), d.getMonth() + 1, d.getDate()];
  }
  const [y, m, d] = iso.split("-").map((s) => parseInt(s, 10));
  return [y || 0, m || 1, d || 1];
}

function partsToIso(y: number, m: number, d: number) {
  const mm = String(m).padStart(2, "0");
  const dd = String(d).padStart(2, "0");
  return `${y}-${mm}-${dd}`;
}

export default function DatePicker({ value, onChange }: DatePickerProps) {
  const { language } = useLanguage();

  if (language === Language.en) {
    // Render three selects for Gregorian date (year, month, day)
    const [gy, gm, gd] = isoToParts(value);

    const currentYear = new Date().getFullYear();
    const startYear = currentYear - 50;
    const endYear = currentYear + 10;

    const isLeap = (y: number) => (y % 4 === 0 && (y % 100 !== 0 || y % 400 === 0));
    const gregorianMonthLengths = (y: number) => [31, isLeap(y) ? 29 : 28, 31,30,31,30,31,31,30,31,30,31];

    return (
      <div className="flex gap-2 items-center mb-3">
        <select
          aria-label="Gregorian year"
          className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
          value={gy}
          onChange={(e) => {
            const newGy = parseInt(e.target.value, 10);
            const maxDay = gregorianMonthLengths(newGy)[gm - 1];
            const newGd = Math.min(gd, maxDay);
            onChange(partsToIso(newGy, gm, newGd));
          }}
        >
          {Array.from({ length: endYear - startYear + 1 }).map((_, i) => {
            const y = startYear + i;
            return (
              <option key={y} value={y}>{y}</option>
            );
          })}
        </select>

        <select
          aria-label="Gregorian month"
          className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
          value={gm}
          onChange={(e) => {
            const newGm = parseInt(e.target.value, 10);
            const maxDay = gregorianMonthLengths(gy)[newGm - 1];
            const newGd = Math.min(gd, maxDay);
            onChange(partsToIso(gy, newGm, newGd));
          }}
        >
          {Array.from({ length: 12 }).map((_, i) => {
            const m = i + 1;
            return (
              <option key={m} value={m}>{m}</option>
            );
          })}
        </select>

        <select
          aria-label="Gregorian day"
          className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
          value={gd}
          onChange={(e) => {
            const newGd = parseInt(e.target.value, 10);
            onChange(partsToIso(gy, gm, newGd));
          }}
        >
          {Array.from({ length: gregorianMonthLengths(gy)[gm - 1] }).map((_, i) => {
            const d = i + 1;
            return (
              <option key={d} value={d}>{d}</option>
            );
          })}
        </select>
      </div>
    );
  }

  // Persian (Jalali) - render three selects
  const [gy, gm, gd] = isoToParts(value);
  const [jy, jm, jd] = gregorianToJalali(gy, gm, gd);

  const currentYear = new Date().getFullYear();
  const jalaliYearNow = gregorianToJalali(currentYear, 1, 1)[0];
  const startYear = jalaliYearNow - 50;
  const endYear = jalaliYearNow + 10;

  const monthLengths = [31,31,31,31,31,31,30,30,30,30,30,29];

  return (
    <div className="flex gap-2 items-center mb-3">
      <select
        aria-label="Jalali year"
        className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
        value={jy}
        onChange={(e) => {
          const newJy = parseInt(e.target.value, 10);
          const [ngy, ngm, ngd] = jalaliToGregorian(newJy, jm, Math.min(jd, monthLengths[jm - 1]));
          onChange(partsToIso(ngy, ngm, ngd));
        }}
      >
        {Array.from({ length: endYear - startYear + 1 }).map((_, i) => {
          const y = startYear + i;
          return (
            <option key={y} value={y}>{y}</option>
          );
        })}
      </select>

      <select
        aria-label="Jalali month"
        className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
        value={jm}
        onChange={(e) => {
          const newJm = parseInt(e.target.value, 10);
          const maxDay = monthLengths[newJm - 1];
          const newJd = Math.min(jd, maxDay);
          const [ngy, ngm, ngd] = jalaliToGregorian(jy, newJm, newJd);
          onChange(partsToIso(ngy, ngm, ngd));
        }}
      >
        {Array.from({ length: 12 }).map((_, i) => {
          const m = i + 1;
          return (
            <option key={m} value={m}>{m}</option>
          );
        })}
      </select>

      <select
        aria-label="Jalali day"
        className="border border-gray-200 dark:border-gray-700 rounded-md p-2"
        value={jd}
        onChange={(e) => {
          const newJd = parseInt(e.target.value, 10);
          const [ngy, ngm, ngd] = jalaliToGregorian(jy, jm, newJd);
          onChange(partsToIso(ngy, ngm, ngd));
        }}
      >
        {Array.from({ length: monthLengths[jm - 1] }).map((_, i) => {
          const d = i + 1;
          return (
            <option key={d} value={d}>{d}</option>
          );
        })}
      </select>
    </div>
  );
}
