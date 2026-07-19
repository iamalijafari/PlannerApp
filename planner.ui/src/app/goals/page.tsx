"use client";

import { useState, useEffect } from "react";
import GoalForm from "@/src/features/goals/components/GoalForm";
import GoalList from "@/src/features/goals/components/GoalList";
import Modal from "@/components/Modal";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { useApiResponse } from "@/utils/use-api-response";
import * as goalsApi from "@/src/features/goals/api/goals-api";

export default function GoalsPage() {
  const [goals, setGoals] = useState<GoalResponseModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMessage, setModalMessage] = useState("");

  const { handleResponse } = useApiResponse();

  // LOAD GOALS
  const loadGoals = async () => {
    try {
      const data = await goalsApi.getGoals();

      await handleResponse<GoalResponseModel[]>(
        data,
        (result) => {
          setGoals(result || []);
        },
        (message) => {
          setGoals([]);
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Failed to load goals:", err);
      setGoals([]);
      setModalMessage("Unexpected error occurred.");
      setModalOpen(true);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadGoals();
  }, []);

  // CREATE GOAL
  const handleCreateGoal = async (model: CreateGoalRequestModel) => {
    try {
      const data = await goalsApi.createGoal(model);

      await handleResponse<GoalResponseModel>(
        data,
        (createdGoal) => setGoals((prev) => [...prev, createdGoal]),
        (message) => {
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Error creating goal:", err);
      setModalMessage("Unexpected error occurred.");
      setModalOpen(true);
    }
  };

  // COMPLETE GOAL
  const handleComplete = async (id: string) => {
    try {
      const data = await goalsApi.completeGoal(id);

      await handleResponse<boolean>(
        data,
        async () => {
          await loadGoals();
        },
        (message) => {
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Error completing goal:", err);
      setModalMessage("Unexpected error occurred.");
      setModalOpen(true);
    }
  };

  // DELETE GOAL
  const handleDelete = async (id: string) => {
    try {
      const data = await goalsApi.deleteGoal(id);

      await handleResponse<boolean>(
        data,
        async () => {
          await loadGoals();
        },
        (message) => {
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Error deleting goal:", err);
      setModalMessage("Unexpected error occurred.");
      setModalOpen(true);
    }
  };

  if (loading) return <p className="p-4">Loading...</p>;

  return (
    <div className="max-w-2xl mx-auto py-8">
      <h1 className="text-3xl font-bold mb-6">Goals</h1>

      <GoalForm onSubmit={handleCreateGoal} />

      <GoalList
        goals={goals}
        onComplete={handleComplete}
        onDelete={handleDelete}
      />

      <Modal isOpen={modalOpen} onClose={() => setModalOpen(false)} title={"Notice"} message={modalMessage} />
    </div>
  );
}