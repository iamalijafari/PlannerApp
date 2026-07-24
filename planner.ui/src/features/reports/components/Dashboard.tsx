"use client";

import { CSSProperties, useCallback, useEffect, useMemo, useState } from "react";
import Link from "next/link";
import Modal from "@/components/Modal";
import { useLanguage } from "@/context/language-context";
import { useTranslation } from "@/context/translation-context";
import { Language } from "@/types/language";
import { MessageKey } from "@/types/message-key";
import { getGoalsProgress } from "../api/report-api";
import {
  GoalProgressModel,
  GoalProgressStatus,
  GoalsProgressReportModel,
} from "../types/goals-progress-report-model";

type GoalFilter = "all" | "active" | "completed" | "overdue" | "planned";

const emptyReport: GoalsProgressReportModel = {
  totalGoals: 0,
  activeGoals: 0,
  completedGoals: 0,
  overdueGoals: 0,
  completedLeafPlans: 0,
  totalLeafPlans: 0,
  overallProgressPercentage: 0,
  goals: [],
};

const statusMessageKeys: Record<GoalProgressStatus, MessageKey> = {
  planned: MessageKey.StatusPlanned,
  "in-progress": MessageKey.StatusInProgress,
  completed: MessageKey.StatusCompleted,
  overdue: MessageKey.StatusOverdue,
};

function SummaryIcon({
  type,
}: {
  type: "total" | "active" | "completed" | "overdue";
}) {
  const paths = {
    total: (
      <>
        <circle cx="12" cy="12" r="8" />
        <circle cx="12" cy="12" r="3" />
      </>
    ),
    active: (
      <>
        <path d="M12 3a9 9 0 1 0 9 9" />
        <path d="M12 7v5l3 2" />
        <path d="M17 3h4v4" />
      </>
    ),
    completed: (
      <>
        <circle cx="12" cy="12" r="9" />
        <path d="m8 12 2.6 2.6L16.5 9" />
      </>
    ),
    overdue: (
      <>
        <path d="M10.3 4.2 2.8 17.1A2 2 0 0 0 4.5 20h15a2 2 0 0 0 1.7-2.9L13.7 4.2a2 2 0 0 0-3.4 0Z" />
        <path d="M12 9v4M12 16.5h.01" />
      </>
    ),
  };

  return (
    <span className={`summary-icon summary-icon-${type}`} aria-hidden="true">
      <svg viewBox="0 0 24 24">{paths[type]}</svg>
    </span>
  );
}

function ProgressRing({
  percentage,
  label,
}: {
  percentage: number;
  label: string;
}) {
  return (
    <div
      className="progress-ring"
      role="progressbar"
      aria-label={label}
      aria-valuemin={0}
      aria-valuemax={100}
      aria-valuenow={percentage}
      style={{ "--progress": `${percentage * 3.6}deg` } as CSSProperties}
    >
      <div>
        <strong>{percentage}%</strong>
        <span>{label}</span>
      </div>
    </div>
  );
}

function matchesFilter(goal: GoalProgressModel, filter: GoalFilter) {
  if (filter === "all") return true;
  if (filter === "active") {
    return goal.status === "planned" || goal.status === "in-progress";
  }
  return goal.status === filter;
}

export default function Dashboard() {
  const { language } = useLanguage();
  const { t } = useTranslation();
  const [report, setReport] = useState<GoalsProgressReportModel>(emptyReport);
  const [isLoading, setIsLoading] = useState(true);
  const [errorKey, setErrorKey] = useState<MessageKey | null>(null);
  const [search, setSearch] = useState("");
  const [filter, setFilter] = useState<GoalFilter>("all");

  const loadReport = useCallback(async () => {
    setIsLoading(true);
    try {
      const response = await getGoalsProgress();
      if (response.success) {
        setReport(response.result ?? emptyReport);
      } else {
        setReport(emptyReport);
        setErrorKey(response.messageKey);
      }
    } catch (error) {
      console.error("Failed to load the goals progress report:", error);
      setReport(emptyReport);
      setErrorKey(MessageKey.ServerError);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadReport();
  }, [loadReport]);

  const dateFormatter = useMemo(
    () =>
      new Intl.DateTimeFormat(
        language === Language.fa ? "fa-IR-u-ca-persian" : "en",
        { dateStyle: "medium" },
      ),
    [language],
  );

  const visibleGoals = useMemo(() => {
    const query = search.trim().toLocaleLowerCase();
    return report.goals.filter((goal) => {
      const matchesSearch =
        !query ||
        goal.title.toLocaleLowerCase().includes(query) ||
        goal.description.toLocaleLowerCase().includes(query);
      return matchesSearch && matchesFilter(goal, filter);
    });
  }, [filter, report.goals, search]);

  const summaryItems = [
    {
      label: t(MessageKey.TotalGoals),
      value: report.totalGoals,
      type: "total" as const,
    },
    {
      label: t(MessageKey.ActiveGoals),
      value: report.activeGoals,
      type: "active" as const,
    },
    {
      label: t(MessageKey.CompletedGoals),
      value: report.completedGoals,
      type: "completed" as const,
    },
    {
      label: t(MessageKey.OverdueGoals),
      value: report.overdueGoals,
      type: "overdue" as const,
    },
  ];

  const filterOptions: Array<{ value: GoalFilter; label: string }> = [
    { value: "all", label: t(MessageKey.FilterAll) },
    { value: "active", label: t(MessageKey.FilterActive) },
    { value: "completed", label: t(MessageKey.FilterCompleted) },
    { value: "overdue", label: t(MessageKey.FilterOverdue) },
    { value: "planned", label: t(MessageKey.FilterPlanned) },
  ];

  return (
    <div className="dashboard-container">
      <section className="dashboard-hero">
        <div>
          <p className="eyebrow">{t(MessageKey.DashboardEyebrow)}</p>
          <h1>{t(MessageKey.DashboardTitle)}</h1>
          <p className="dashboard-intro">
            {t(MessageKey.DashboardDescription)}
          </p>
        </div>
        <Link href="/goals" className="hero-action">
          <svg viewBox="0 0 24 24" aria-hidden="true">
            <path d="M12 5v14M5 12h14" />
          </svg>
          {t(MessageKey.GoalFormTitle)}
        </Link>
      </section>

      {isLoading ? (
        <div className="dashboard-loading" aria-label={t(MessageKey.Loading)}>
          <div />
          <div />
          <div />
          <div />
        </div>
      ) : (
        <>
          <section className="summary-grid" aria-label={t(MessageKey.Dashboard)}>
            {summaryItems.map((item) => (
              <article className="summary-card" key={item.type}>
                <SummaryIcon type={item.type} />
                <div>
                  <span>{item.label}</span>
                  <strong>{item.value}</strong>
                </div>
              </article>
            ))}
          </section>

          <section className="progress-overview">
            <ProgressRing
              percentage={report.overallProgressPercentage}
              label={t(MessageKey.OverallProgress)}
            />
            <div className="progress-overview-copy">
              <p className="eyebrow">{t(MessageKey.OverallProgress)}</p>
              <h2>
                {t(MessageKey.CompletedPlans, {
                  completed: report.completedLeafPlans,
                  total: report.totalLeafPlans,
                })}
              </h2>
              <p>{t(MessageKey.DashboardDescription)}</p>
            </div>
            <div className="progress-legend" aria-hidden="true">
              <span />
              <span />
              <span />
              <span />
              <span />
            </div>
          </section>

          <section className="goals-report" aria-labelledby="goals-progress-title">
            <div className="report-toolbar">
              <div>
                <p className="eyebrow">{t(MessageKey.DashboardEyebrow)}</p>
                <h2 id="goals-progress-title">{t(MessageKey.GoalsProgress)}</h2>
              </div>

              <div className="report-controls">
                <label className="search-control">
                  <span className="sr-only">{t(MessageKey.SearchGoals)}</span>
                  <svg viewBox="0 0 24 24" aria-hidden="true">
                    <circle cx="11" cy="11" r="7" />
                    <path d="m16 16 4 4" />
                  </svg>
                  <input
                    type="search"
                    value={search}
                    onChange={(event) => setSearch(event.target.value)}
                    placeholder={t(MessageKey.SearchGoals)}
                  />
                </label>

                <label>
                  <span className="sr-only">{t(MessageKey.FilterAll)}</span>
                  <select
                    value={filter}
                    onChange={(event) =>
                      setFilter(event.target.value as GoalFilter)
                    }
                  >
                    {filterOptions.map((option) => (
                      <option key={option.value} value={option.value}>
                        {option.label}
                      </option>
                    ))}
                  </select>
                </label>
              </div>
            </div>

            {report.goals.length === 0 ? (
              <div className="report-empty">
                <span className="brand-orbit" aria-hidden="true">
                  <span />
                </span>
                <h3>{t(MessageKey.Goals_Empty)}</h3>
                <Link href="/goals" className="btn">
                  {t(MessageKey.GoalFormTitle)}
                </Link>
              </div>
            ) : visibleGoals.length === 0 ? (
              <div className="report-empty">
                <h3>{t(MessageKey.NoMatchingGoals)}</h3>
              </div>
            ) : (
              <div className="goal-progress-grid">
                {visibleGoals.map((goal) => (
                  <article className="goal-progress-card" key={goal.id}>
                    <div className="goal-card-heading">
                      <span className={`status-pill status-${goal.status}`}>
                        <span />
                        {t(statusMessageKeys[goal.status])}
                      </span>
                      <span className="goal-due-date">
                        {t(MessageKey.DueDate)}{" "}
                        {dateFormatter.format(new Date(goal.dueDate))}
                      </span>
                    </div>

                    <div className="goal-card-copy">
                      <h3>{goal.title}</h3>
                      {goal.description && <p>{goal.description}</p>}
                    </div>

                    <div className="goal-progress-details">
                      <div>
                        <span>{t(MessageKey.Progress)}</span>
                        <strong>{goal.progressPercentage}%</strong>
                      </div>
                      <div
                        className="goal-progress-track"
                        role="progressbar"
                        aria-label={`${goal.title}: ${goal.progressPercentage}%`}
                        aria-valuemin={0}
                        aria-valuemax={100}
                        aria-valuenow={goal.progressPercentage}
                      >
                        <span
                          style={{ width: `${goal.progressPercentage}%` }}
                        />
                      </div>
                      <p>
                        {t(MessageKey.CompletedPlans, {
                          completed: goal.completedLeafPlans,
                          total: goal.totalLeafPlans,
                        })}
                      </p>
                    </div>

                    <div className="goal-card-actions">
                      <Link href={`/goals/${goal.id}/tree`}>
                        {t(MessageKey.OpenPlan)}
                        <svg viewBox="0 0 24 24" aria-hidden="true">
                          <path d="m9 18 6-6-6-6" />
                        </svg>
                      </Link>
                      <Link href={`/goals/${goal.id}/edit`}>
                        {t(MessageKey.ManageGoal)}
                      </Link>
                    </div>
                  </article>
                ))}
              </div>
            )}
          </section>
        </>
      )}

      <Modal
        isOpen={errorKey !== null}
        onClose={() => setErrorKey(null)}
        title={t(MessageKey.ErrorTitle)}
        message={errorKey === null ? "" : t(errorKey)}
        closeLabel={t(MessageKey.Close)}
      />
    </div>
  );
}
