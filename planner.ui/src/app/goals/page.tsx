"use client";

import { useCallback, useEffect, useState } from "react";
import Modal from "@/components/Modal";
import { useTranslation } from "@/context/translation-context";
import * as goalsApi from "@/features/goals/api/goals-api";
import GoalForm from "@/features/goals/components/GoalForm";
import GoalList from "@/features/goals/components/GoalList";
import { CreateGoalRequestModel } from "@/features/goals/types/create-goal-request-model";
import { GoalResponseModel } from "@/features/goals/types/goal-response-model";
import { MessageKey } from "@/types/message-key";

export default function GoalsPage() {
  const { t } = useTranslation();
  const [goals, setGoals] = useState<GoalResponseModel[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorKey, setErrorKey] = useState<MessageKey | null>(null);

  const loadGoals = useCallback(async () => {
    try {
      const response = await goalsApi.getGoals();
      if (response.success) {
        setGoals(response.result ?? []);
      } else {
        setGoals([]);
        setErrorKey(response.messageKey);
      }
    } catch (error) {
      console.error("Failed to load goals:", error);
      setGoals([]);
      setErrorKey(MessageKey.ServerError);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadGoals();
  }, [loadGoals]);

  const handleCreateGoal = async (model: CreateGoalRequestModel) => {
    try {
      const response = await goalsApi.createGoal(model);
      if (!response.success) {
        setErrorKey(response.messageKey);
        return false;
      }

      setGoals((current) => [...current, response.result]);
      return true;
    } catch (error) {
      console.error("Failed to create goal:", error);
      setErrorKey(MessageKey.ServerError);
      return false;
    }
  };

  const handleComplete = async (id: string) => {
    try {
      const response = await goalsApi.completeGoal(id);
      if (!response.success) {
        setErrorKey(response.messageKey);
        return false;
      }

      await loadGoals();
      return true;
    } catch (error) {
      console.error("Failed to complete goal:", error);
      setErrorKey(MessageKey.ServerError);
      return false;
    }
  };

  const handleDelete = async (id: string) => {
    try {
      const response = await goalsApi.deleteGoal(id);
      if (!response.success) {
        setErrorKey(response.messageKey);
        return false;
      }

      setGoals((current) => current.filter((goal) => goal.id !== id));
      return true;
    } catch (error) {
      console.error("Failed to delete goal:", error);
      setErrorKey(MessageKey.ServerError);
      return false;
    }
  };

  return (
    <div className="app-container space-y-8">
      <GoalForm
        heading={t(MessageKey.GoalFormTitle)}
        onSubmit={handleCreateGoal}
        resetOnSuccess
      />

      {isLoading ? (
        <p className="status-message">{t(MessageKey.Loading)}</p>
      ) : (
        <GoalList
          goals={goals}
          onComplete={handleComplete}
          onDelete={handleDelete}
        />
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
