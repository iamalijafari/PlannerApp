"use client";

import { useState, useEffect } from "react";
import GoalForm from "@/src/features/goals/components/GoalForm";
import GoalList from "@/src/features/goals/components/GoalList";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import Modal from "@/components/Modal";
import { useApiResponse } from "@/utils/use-api-response";

const API_BASE = "http://localhost:5010/api/goal";

export default function GoalsPage() {
  const [goals, setGoals] = useState<GoalResponseModel[]>([]);
  const [loading, setLoading] = useState(true);

  const [modalOpen, setModalOpen] = useState(false);
  const [modalMessage, setModalMessage] = useState("");

  const { handleResponse } = useApiResponse();

  // LOAD GOALS
  const loadGoals = async () => {
    try {
      const res = await fetch(API_BASE);
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
      const res = await fetch(API_BASE, {
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
      const res = await fetch(`${API_BASE}/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id, isCompleted: true }),
      });

      const data = await res.json();

      await handleResponse<boolean>(
        data,
        () => {
          setGoals((prev) =>
            prev.map((g) => (g.Id === id ? { ...g, IsCompleted: true } : g))
          );
        },
        (message) => {
          setModalMessage(message);
          setModalOpen(true);
        }
      );
    } catch (err) {
      console.error("Error updating goal:", err);
      setModalMessage("Unexpected error occurred.");
      setModalOpen(true);
    }
  };

  // DELETE GOAL
  const handleDelete = async (id: string) => {
    try {
      const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
      const data = await res.json();

      await handleResponse<boolean>(
        data,
        () => {
          setGoals((prev) => prev.filter((g) => g.Id !== id));
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

      <Modal
        isOpen={modalOpen}
        onClose={() => setModalOpen(false)}
        title="Error"
        message={modalMessage}
      />
    </div>
  );
}