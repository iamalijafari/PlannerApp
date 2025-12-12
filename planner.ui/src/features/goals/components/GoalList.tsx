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
      <h2>{t(MessageKey.GoalListTitle)}</h2>
      {goals.map((g) => (
        <div key={g.id}>
          <h3>{g.title}</h3>
          <p>{g.description}</p>
          <p>{t(MessageKey.DueDate)}: {g.dueDate}</p>

          {!g.isCompleted && <button onClick={() => handleCompleteClick(g.id)}>{t(MessageKey.Complete)}</button>}
          <button onClick={() => handleDeleteClick(g.id)}>{t(MessageKey.Delete)}</button>
        </div>
      ))}

      <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} title={t(MessageKey.ErrorTitle)} message={modalMessage} />
    </>
  );
}