"use client";

import { useState } from "react";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import Modal from "@/components/Modal";
import Link from "next/link";
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from "@/types/message-key";

interface GoalListProps {
  goals: GoalResponseModel[];
  onComplete: (id: string) => void | Promise<void>;
  onDelete: (id: string) => void | Promise<void>;
}

export default function GoalList({ goals, onComplete, onDelete }: GoalListProps) {
  const { t } = useTranslation();
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMessage, setModalMessage] = useState("");

  const handleCompleteClick = async (id: string) => {
    try { await onComplete(id); } 
    catch (err: unknown) {
      setModalMessage(err instanceof Error ? err.message : t(MessageKey.ServerError));
      setModalOpen(true);
    }
  };

  const handleDeleteClick = async (id: string) => {
    try { await onDelete(id); } 
    catch (err: unknown) {
      setModalMessage(err instanceof Error ? err.message : t(MessageKey.ServerError));
      setModalOpen(true);
    }
  };

  return (
    <>
      <h2 className="text-2xl font-semibold mb-4">{t(MessageKey.GoalListTitle)}</h2>

      <div className="space-y-4">
        {goals.map((g) => (
          <div
            key={g.id}
            className={`p-4 rounded-lg shadow-sm bg-white dark:bg-[#0b0b0b] flex items-start justify-between ${g.isCompleted ? "opacity-60" : ""}`}
          >
            <div className="pr-4">
              <h3 className={`text-lg font-medium ${g.isCompleted ? "line-through" : ""}`}>{g.title}</h3>
              <p className="text-sm text-gray-600 dark:text-gray-300">{g.description}</p>
              <p className="text-xs text-gray-500 dark:text-gray-400 mt-2">{t(MessageKey.DueDate)}: {g.dueDate}</p>
            </div>

            <div className="flex flex-col items-end gap-2">
              <Link href={`/goals/${g.id}/edit`} className="muted-btn text-sm">{t(MessageKey.Edit)}</Link>
              <Link href={`/goals/${g.id}/tree`} className="muted-btn text-sm">{t(MessageKey.Edit)} {t(MessageKey.Plan)}</Link>
              {!g.isCompleted && (
                <button className="btn text-sm" onClick={() => handleCompleteClick(g.id)}>{t(MessageKey.Complete)}</button>
              )}
              <button
                className="muted-btn text-sm"
                onClick={() => handleDeleteClick(g.id)}
              >
                {t(MessageKey.Delete)}
              </button>
            </div>
          </div>
        ))}
      </div>

      <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} title={t(MessageKey.ErrorTitle)} message={modalMessage} />
    </>
  );
}