"use client";

import { CreateGoalRequestModel } from "@/src/features/goals/types/create-goal-request-model";
import { GoalResponseModel } from "@/src/features/goals/types/goal-response-model";
import { ResponseModel } from "@/types/response-model";
import { MessageKey } from "@/types/message-key";

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5010/api";
const API_GOAL_ENDPOINT = `${API_BASE_URL}/goal`;

async function handleResponse<T>(response: Response): Promise<ResponseModel<T>> {
  if (!response.ok) {
    throw new Error(`HTTP ${response.status}: ${response.statusText}`);
  }
  return response.json();
}

export async function getGoals(): Promise<ResponseModel<GoalResponseModel[]>> {
  try {
    const res = await fetch(API_GOAL_ENDPOINT, { 
      cache: "no-store",
      headers: { "Content-Type": "application/json" }
    });
    return await handleResponse(res);
  } catch (error) {
    console.error("Failed to fetch goals:", error);
    return {
      success: false,
      messageKey: MessageKey.ServerError,
      result: []
    };
  }
}

export async function getGoal(id: string): Promise<ResponseModel<GoalResponseModel>> {
  try {
    if (!id) {
      throw new Error("Goal ID is required");
    }
    const res = await fetch(`${API_GOAL_ENDPOINT}/${id}`, { 
      cache: "no-store",
      headers: { "Content-Type": "application/json" }
    });
    return await handleResponse(res);
  } catch (error) {
    console.error(`Failed to fetch goal ${id}:`, error);
    return {
      success: false,
      messageKey: MessageKey.Goal_NotFound,
      result: null as any
    };
  }
}

export async function createGoal(
  request: CreateGoalRequestModel
): Promise<ResponseModel<GoalResponseModel>> {
  try {
    if (!request || !request.title) {
      throw new Error("Title is required");
    }
    const res = await fetch(API_GOAL_ENDPOINT, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(request),
    });
    return await handleResponse(res);
  } catch (error) {
    console.error("Failed to create goal:", error);
    return {
      success: false,
      messageKey: MessageKey.Operation_Failed,
      result: null as any
    };
  }
}

export async function updateGoal(
  id: string,
  request: Partial<CreateGoalRequestModel> & { isCompleted?: boolean }
): Promise<ResponseModel<boolean>> {
  try {
    if (!id) {
      throw new Error("Goal ID is required");
    }
    const res = await fetch(`${API_GOAL_ENDPOINT}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(request),
    });
    return await handleResponse(res);
  } catch (error) {
    console.error(`Failed to update goal ${id}:`, error);
    return {
      success: false,
      messageKey: MessageKey.Operation_Failed,
      result: false
    };
  }
}

export async function deleteGoal(id: string): Promise<ResponseModel<boolean>> {
  try {
    if (!id) {
      throw new Error("Goal ID is required");
    }
    const res = await fetch(`${API_GOAL_ENDPOINT}/${id}`, {
      method: "DELETE",
      headers: { "Content-Type": "application/json" }
    });
    return await handleResponse(res);
  } catch (error) {
    console.error(`Failed to delete goal ${id}:`, error);
    return {
      success: false,
      messageKey: MessageKey.Operation_Failed,
      result: false
    };
  }
}

export async function completeGoal(
  id: string
): Promise<ResponseModel<boolean>> {
  try {
    if (!id) {
      throw new Error("Goal ID is required");
    }
    const res = await fetch(`${API_GOAL_ENDPOINT}/${id}/complete`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" }
    });
    return await handleResponse(res);
  } catch (error) {
    console.error(`Failed to update goal ${id}:`, error);
    return {
      success: false,
      messageKey: MessageKey.Operation_Failed,
      result: false
    };
  }
}