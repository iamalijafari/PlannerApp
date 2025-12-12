"use client";

import { useState } from "react";
import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { createGoal } from "../api/goals-api";
import { useApiResponse } from "@/utils/use-api-response";
import Modal from "@/components/Modal";
import { useTranslation } from "@/context/translationContext";
import { MessageKey } from "@/types/message-key";

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

    const request: CreateGoalRequestModel = {
      Title: title,
      Description: description,
      DueDate: dueDate,
    };

    try {
      const response = await createGoal(request);

      await handleResponse(
        response,
        async () => {
          await onSubmit(request);
          setTitle("");
          setDescription("");
          setDueDate("");
        },
        (translatedMessage) => {
          setModalMessage(translatedMessage);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error(err);
      setModalMessage(t(MessageKey.ServerError));
      setModalOpen(true);
    }
  };

  return (
    <>
      <form onSubmit={submitHandler} className="bg-white p-4 rounded shadow-md mb-4">
        <h2 className="text-xl font-bold mb-4">{t(MessageKey.GoalFormTitle)}</h2>

        <div className="mb-3">
          <label className="block mb-1 font-medium">{t(MessageKey.Title)}</label>
          <input
            className="border border-gray-300 rounded p-2 w-full"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="block mb-1 font-medium">{t(MessageKey.Description)}</label>
          <textarea
            className="border border-gray-300 rounded p-2 w-full"
            rows={3}
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="block mb-1 font-medium">{t(MessageKey.DueDate)}</label>
          <input
            type="date"
            className="border border-gray-300 rounded p-2 w-full"
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
            required
          />
        </div>

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          {t(MessageKey.Save)}
        </button>
      </form>

      <Modal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        title={t(MessageKey.ErrorTitle)}
        message={modalMessage}
      />
    </>
  );
}