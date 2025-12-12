"use client";

import { useState, useEffect } from "react";
import GoalForm from "@/src/features/goals/components/GoalForm";
import GoalList from "@/src/features/goals/components/GoalList";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { useApiResponse } from "@/utils/use-api-response";

const API_BASE = process.env.NEXT_PUBLIC_GOAL_API_URL;

if (!API_BASE) {
  throw new Error("NEXT_PUBLIC_GOAL_API_URL is not defined in .env.local");
}

export default function GoalsPage() {
  const [goals, setGoals] = useState<GoalResponseModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [modalMessage, setModalMessage] = useState("");

  const { handleResponse } = useApiResponse();

  // LOAD GOALS
  const loadGoals = async () => {
    try {
      const res = await fetch(`${API_BASE}/GetAll`, { method: "POST" });
      const data = await res.json();

      await handleResponse<GoalResponseModel[]>(
        data,
        (result) => setGoals(result),
        (message) => {
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Failed to load goals:", err);
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
      const res = await fetch(`${API_BASE}/Create`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(model),
      });

      const data = await res.json();

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
      const res = await fetch(`${API_BASE}/Complete`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(id),
      });

      const data = await res.json();

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

  // DELETE GOAL
  const handleDelete = async (id: string) => {
    try {
      const res = await fetch(`${API_BASE}/Delete`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(id),
      });

      const data = await res.json();

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

      {modalOpen && (
        <div>{modalMessage}</div>
      )}
    </div>
  );
}