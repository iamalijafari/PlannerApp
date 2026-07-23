"use client";

import { useParams, useRouter } from "next/navigation";
import { useCallback, useEffect, useState } from "react";
import Modal from "@/components/Modal";
import { useTranslation } from "@/context/translation-context";
import { getGoal, updateGoal } from "@/features/goals/api/goals-api";
import GoalForm from "@/features/goals/components/GoalForm";
import { CreateGoalRequestModel } from "@/features/goals/types/create-goal-request-model";
import { GoalResponseModel } from "@/features/goals/types/goal-response-model";
import { MessageKey } from "@/types/message-key";

export default function EditGoalPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { t } = useTranslation();
  const [goal, setGoal] = useState<GoalResponseModel | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [errorKey, setErrorKey] = useState<MessageKey | null>(null);

  const loadGoal = useCallback(async () => {
    try {
      const response = await getGoal(id);
      if (response.success) {
        setGoal(response.result);
      } else {
        setErrorKey(response.messageKey);
      }
    } catch (error) {
      console.error(`Failed to load goal ${id}:`, error);
      setErrorKey(MessageKey.ServerError);
    } finally {
      setIsLoading(false);
    }
  }, [id]);

  useEffect(() => {
    void loadGoal();
  }, [loadGoal]);

  const handleSubmit = async (model: CreateGoalRequestModel) => {
    if (!goal) return false;

    try {
      const response = await updateGoal(id, {
        ...model,
        isCompleted: goal.isCompleted,
      });
      if (!response.success) {
        setErrorKey(response.messageKey);
        return false;
      }

      router.push("/goals");
      return true;
    } catch (error) {
      console.error(`Failed to update goal ${id}:`, error);
      setErrorKey(MessageKey.ServerError);
      return false;
    }
  };

  return (
    <div className="app-container">
      {isLoading ? (
        <p className="status-message">{t(MessageKey.Loading)}</p>
      ) : goal ? (
        <GoalForm
          heading={t(MessageKey.Goal_Edit_Button)}
          initialValue={{
            title: goal.title,
            description: goal.description,
            dueDate: goal.dueDate.slice(0, 10),
          }}
          onSubmit={handleSubmit}
          onCancel={() => router.push("/goals")}
        />
      ) : (
        <div className="empty-state">{t(MessageKey.Goal_NotFound)}</div>
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
