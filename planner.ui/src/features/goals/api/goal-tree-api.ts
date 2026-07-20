"use client";

import { ResponseModel } from "@/types/response-model";
import { MessageKey } from "@/types/message-key";
import { GoalTreeModel } from "@/src/features/goals/types/goal-tree-model";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5010/api";

export async function getGoalTree(goalId: string): Promise<ResponseModel<GoalTreeModel>> {
  try {
    const res = await fetch(`${API_BASE_URL}/goal/${goalId}/tree`, {
      cache: "no-store",
      headers: { "Content-Type": "application/json" },
    });
    if (!res.ok) throw new Error(`HTTP ${res.status}: ${res.statusText}`);
    return await res.json();
  } catch (error) {
    console.error(`Failed to fetch goal tree ${goalId}:`, error);
    return { success: false, messageKey: MessageKey.ServerError, result: null as any };
  }
}