"use client";

import Link from "next/link";
import { useState } from "react";
import { useLanguage } from "@/context/language-context";
import { useTranslation } from "@/context/translation-context";
import { Language } from "@/types/language";
import { MessageKey } from "@/types/message-key";
import { GoalResponseModel } from "../types/goal-response-model";

interface GoalListProps {
  goals: GoalResponseModel[];
  onComplete: (id: string) => Promise<boolean>;
  onDelete: (id: string) => Promise<boolean>;
}

export default function GoalList({
  goals,
  onComplete,
  onDelete,
}: GoalListProps) {
  const { language } = useLanguage();
  const { t } = useTranslation();
  const [busyGoalId, setBusyGoalId] = useState<string | null>(null);

  const dateFormatter = new Intl.DateTimeFormat(
    language === Language.fa ? "fa-IR-u-ca-persian" : "en",
    { dateStyle: "medium" },
  );

  const runAction = async (
    goalId: string,
    action: (id: string) => Promise<boolean>,
  ) => {
    setBusyGoalId(goalId);
    try {
      await action(goalId);
    } finally {
      setBusyGoalId(null);
    }
  };

  const handleDelete = async (goal: GoalResponseModel) => {
    if (!window.confirm(t(MessageKey.Goal_Delete_Confirm))) return;
    await runAction(goal.id, onDelete);
  };

  return (
    <section aria-labelledby="goal-list-title">
      <h2 id="goal-list-title" className="mb-4 text-2xl font-semibold">
        {t(MessageKey.GoalListTitle)}
      </h2>

      {goals.length === 0 ? (
        <div className="empty-state">{t(MessageKey.Goals_Empty)}</div>
      ) : (
        <div className="space-y-4">
          {goals.map((goal) => {
            const isBusy = busyGoalId === goal.id;
            return (
              <article
                key={goal.id}
                className={`goal-card ${goal.isCompleted ? "opacity-65" : ""}`}
              >
                <div className="min-w-0 flex-1">
                  <h3
                    className={`text-lg font-semibold ${
                      goal.isCompleted ? "line-through" : ""
                    }`}
                  >
                    {goal.title}
                  </h3>
                  {goal.description && (
                    <p className="mt-1 text-sm text-zinc-600 dark:text-zinc-300">
                      {goal.description}
                    </p>
                  )}
                  <p className="mt-3 text-xs text-zinc-500">
                    {t(MessageKey.DueDate)}:{" "}
                    {dateFormatter.format(new Date(goal.dueDate))}
                  </p>
                </div>

                <div className="actions">
                  <Link
                    href={`/goals/${goal.id}/edit`}
                    className="muted-btn text-sm"
                  >
                    {t(MessageKey.Edit)}
                  </Link>
                  <Link
                    href={`/goals/${goal.id}/tree`}
                    className="muted-btn text-sm"
                  >
                    {t(MessageKey.Plan)}
                  </Link>
                  {!goal.isCompleted && (
                    <button
                      className="btn text-sm"
                      disabled={isBusy}
                      onClick={() => void runAction(goal.id, onComplete)}
                    >
                      {t(MessageKey.Complete)}
                    </button>
                  )}
                  <button
                    className="danger-btn text-sm"
                    disabled={isBusy}
                    onClick={() => void handleDelete(goal)}
                  >
                    {t(MessageKey.Delete)}
                  </button>
                </div>
              </article>
            );
          })}
        </div>
      )}
    </section>
  );
}
