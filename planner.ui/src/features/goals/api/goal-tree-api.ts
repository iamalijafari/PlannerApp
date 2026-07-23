import { apiRequest } from "@/services/api-client";
import { ResponseModel } from "@/types/response-model";
import { GoalTreeModel } from "../types/goal-tree-model";

export function getGoalTree(
  goalId: string,
): Promise<ResponseModel<GoalTreeModel>> {
  return apiRequest<GoalTreeModel>(`/goal/${goalId}/tree`);
}
