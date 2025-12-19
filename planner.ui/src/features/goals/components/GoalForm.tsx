"use client";

import { useState } from "react";
import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { useApiResponse } from "@/utils/use-api-response";
import Modal from "@/components/Modal";
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from "@/types/message-key";
import DatePicker from "@/components/DatePicker";

interface GoalFormProps {
  onSubmit: (model: CreateGoalRequestModel) => void | Promise<void>;
}

export default function GoalForm({ onSubmit }: GoalFormProps) {
  const { t } = useTranslation();
  const { handleResponse } = useApiResponse();

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [dueDate, setDueDate] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMessage, setModalMessage] = useState("");

  const submitHandler = async (e: React.FormEvent) => {
    e.preventDefault();

    const request: CreateGoalRequestModel = { title: title, description: description, dueDate: dueDate };

    try {
      await onSubmit(request);
      setTitle("");
      setDescription("");
      setDueDate("");
    } catch (err) {
      console.error(err);
      setModalMessage(t(MessageKey.ServerError));
      setModalOpen(true);
    }
  };

  return (
    <>
      <form onSubmit={submitHandler} className="card mb-6">
        <h2 className="text-xl font-bold mb-4">{t(MessageKey.GoalFormTitle)}</h2>

        <input
          className="w-full border border-gray-200 dark:border-gray-700 rounded-md p-2 mb-3 bg-transparent"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
          placeholder={t(MessageKey.Title)}
        />

        <textarea
          className="w-full border border-gray-200 dark:border-gray-700 rounded-md p-2 mb-3 bg-transparent"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
          placeholder={t(MessageKey.Description)}
        />

        <DatePicker value={dueDate} onChange={(d) => setDueDate(d)} />

        <div className="flex justify-end">
          <button type="submit" className="btn">{t(MessageKey.Save)}</button>
        </div>
      </form>

      <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} title={t(MessageKey.ErrorTitle)} message={modalMessage} />
    </>
  );
}