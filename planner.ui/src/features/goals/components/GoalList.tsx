"use client";

import { useState } from "react";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import Modal from "@/components/Modal";
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
    try {
      await onComplete(id);
    } catch (err: unknown) {
      const message =
        err instanceof Error ? err.message : t(MessageKey.ServerError);
      setModalMessage(message);
      setModalOpen(true);
    }
  };

  const handleDeleteClick = async (id: string) => {
    try {
      await onDelete(id);
    } catch (err: unknown) {
      const message =
        err instanceof Error ? err.message : t(MessageKey.ServerError);
      setModalMessage(message);
      setModalOpen(true);
    }
  };

  return (
    <>
      <h2 className="text-xl font-bold mb-4">{t(MessageKey.GoalListTitle)}</h2>

      <div className="space-y-3">
        {goals.map((g) => (
          <div key={g.Id} className="p-4 border rounded shadow-sm bg-white">
            <h3 className="font-bold text-lg">{g.Title}</h3>
            <p className="text-sm text-gray-700">{g.Description}</p>
            <p className="text-xs text-gray-500">
              {t(MessageKey.DueDate)}: {g.DueDate}
            </p>

            {!g.IsCompleted && (
              <button
                onClick={() => handleCompleteClick(g.Id)}
                className="mt-2 bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700 mr-2"
              >
                {t(MessageKey.Complete)}
              </button>
            )}

            <button
              onClick={() => handleDeleteClick(g.Id)}
              className="mt-2 bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
            >
              {t(MessageKey.Delete)}
            </button>
          </div>
        ))}
      </div>

      <Modal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        title={t(MessageKey.ErrorTitle)}
        message={modalMessage}
      />
    </>
  );
}