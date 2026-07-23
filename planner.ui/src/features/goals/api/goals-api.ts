import { apiRequest } from "@/services/api-client";
import { ResponseModel } from "@/types/response-model";
import { CreateGoalRequestModel } from "../types/create-goal-request-model";
import { GoalResponseModel } from "../types/goal-response-model";

export interface UpdateGoalRequestModel extends CreateGoalRequestModel {
  isCompleted: boolean;
}

export function getGoals(): Promise<ResponseModel<GoalResponseModel[]>> {
  return apiRequest<GoalResponseModel[]>("/goal/GetAll");
}

export function getGoal(
  id: string,
): Promise<ResponseModel<GoalResponseModel>> {
  return apiRequest<GoalResponseModel>(`/goal/${id}`);
}

export function createGoal(
  request: CreateGoalRequestModel,
): Promise<ResponseModel<GoalResponseModel>> {
  return apiRequest<GoalResponseModel>("/goal", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export function updateGoal(
  id: string,
  request: UpdateGoalRequestModel,
): Promise<ResponseModel<boolean>> {
  return apiRequest<boolean>(`/goal/${id}`, {
    method: "PUT",
    body: JSON.stringify(request),
  });
}

export function deleteGoal(id: string): Promise<ResponseModel<boolean>> {
  return apiRequest<boolean>(`/goal/${id}`, { method: "DELETE" });
}

export function completeGoal(id: string): Promise<ResponseModel<boolean>> {
  return apiRequest<boolean>(`/goal/${id}/complete`, { method: "PUT" });
}
