"use client";

import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import { ResponseModel } from "@/types/response-model";

const API_BASE = "http://localhost:5010/api/goal";

export async function getGoals(): Promise<ResponseModel<GoalResponseModel[]>> {
  const res = await fetch(API_BASE, { cache: "no-store" });
  return res.json();
}

export async function getGoal(id: string): Promise<ResponseModel<GoalResponseModel>> {
  const res = await fetch(`${API_BASE}/${id}`, { cache: "no-store" });
  return res.json();
}

export async function createGoal(
  request: CreateGoalRequestModel
): Promise<ResponseModel<GoalResponseModel>> {
  const res = await fetch(API_BASE, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });
  return res.json();
}

export async function updateGoal(
  id: string,
  request: Partial<CreateGoalRequestModel>
): Promise<ResponseModel<boolean>> {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(request),
  });
  return res.json();
}

export async function deleteGoal(id: string): Promise<ResponseModel<boolean>> {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: "DELETE",
  });
  return res.json();
}