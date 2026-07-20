"use client";

import { useCallback, useEffect, useState } from "react";
import { getGoalTree } from "@/src/features/goals/api/goal-tree-api";
import { GoalTreeModel } from "@/src/features/goals/types/goal-tree-model";
import {
  yearlyGoalApi,
  monthlyGoalApi,
  weeklyGoalApi,
  dailyGoalApi,
} from "@/src/features/goals/api/goal-level-apis";
import GoalTreeNode, { LevelConfig } from "./GoalTreeNode";

const LEVELS: LevelConfig[] = [
  { api: yearlyGoalApi, label: "Yearly", childField: "monthlyGoals" },
  { api: monthlyGoalApi, label: "Monthly", childField: "weeklyGoals" },
  { api: weeklyGoalApi, label: "Weekly", childField: "dailyGoals" },
  { api: dailyGoalApi, label: "Daily", childField: null },
];

interface GoalTreeProps {
  goalId: string;
}

export default function GoalTree({ goalId }: GoalTreeProps) {
  const [tree, setTree] = useState<GoalTreeModel | null>(null);
  const [loading, setLoading] = useState(true);
  const [addingYearly, setAddingYearly] = useState(false);
  const [yTitle, setYTitle] = useState("");
  const [yDescription, setYDescription] = useState("");
  const [yDueDate, setYDueDate] = useState("");

  const load = useCallback(async () => {
    const res = await getGoalTree(goalId);
    if (res.success) setTree(res.result);
    setLoading(false);
  }, [goalId]);

  useEffect(() => {
    load();
  }, [load]);

  async function handleAddYearly() {
    if (!yTitle.trim()) return;
    await yearlyGoalApi.create({ parentId: goalId, title: yTitle, description: yDescription, dueDate: yDueDate });
    setYTitle("");
    setYDescription("");
    setYDueDate("");
    setAddingYearly(false);
    load();
  }

  if (loading) return <p className="p-4">Loading...</p>;
  if (!tree) return <p className="p-4">Goal not found.</p>;

  return (
    <div className="card">
      <h2 className="text-xl font-bold mb-4">{tree.title}</h2>

      <button className="btn text-sm mb-4" onClick={() => setAddingYearly((v) => !v)}>
        + Add Yearly Goal
      </button>

      {addingYearly && (
        <div className="flex flex-col gap-2 mb-4 max-w-sm">
          <input placeholder="Yearly goal title" value={yTitle} onChange={(e) => setYTitle(e.target.value)} />
          <textarea placeholder="Description" value={yDescription} onChange={(e) => setYDescription(e.target.value)} />
          <input type="date" value={yDueDate} onChange={(e) => setYDueDate(e.target.value)} />
          <div className="flex gap-2">
            <button className="btn text-sm" disabled={!yTitle.trim()} onClick={handleAddYearly}>Add</button>
            <button className="muted-btn text-sm" onClick={() => setAddingYearly(false)}>Cancel</button>
          </div>
        </div>
      )}

      {tree.yearlyGoals.length === 0 && <p className="text-sm text-zinc-500">No yearly goals yet.</p>}

      {tree.yearlyGoals.map((y) => (
        <GoalTreeNode key={y.id} node={y} depth={0} levels={LEVELS} onChanged={load} />
      ))}
    </div>
  );
}